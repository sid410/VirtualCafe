using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


public class pythonProgram : MonoBehaviour
{
    //pythonがある場所
    private string pyExePath = @"C:/Users/imd-lab/AppData/Local/Programs/Python/Python39/python.exe";

    //実行したいスクリプトがある場所
    private string pyCodePath = @"C:/Users/imd-lab/Desktop/2022TSUNDERE/VirtualCafe/AvatarReactUnity/Assets/Scripts/Python/code.py";

    //エージェント切り替えのため使用
    public main main;
    //テキストの加工前の一行を入れる変数
    public string[] textMessage;
 
    
    //非同期処理（pythonコードの処理が重いので）
    public async void ChangeBehaviourBasedOnQuestionnaire()
    {
        string str = "";
        //事前アンケート結果の読み込み
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("Questionnaire/qResult", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        textMessage = TextLines.Split('\n');
        if (textMessage[textMessage.Length - 1] == "")
        {
            textMessage = textMessage.SkipLast(1).ToArray();
            print("The last line is null.");
        }
        int textlength = textMessage.Length;
        string[] qResult = textMessage[textlength - 1].Split(',');
        if (main == null) main = this.GetComponent<main>();
        await Task.Run(() =>
        {
            // これは別スレッドで実施
            str = PythonCode(qResult[0], qResult[1], qResult[2], qResult[3]);
            return true;
        });
        //以降はメインスレッドで実施
        switch (qResult[4])//Agent
        {
            case "0"://Kaguya
                main.SetAgent(0);
                main.SetAgentVoice(0);
                main.SetDialogue(0);
                main.checkAgent.text = "Agent: Kaguya";
                break;
            case "1"://Manaka
                main.SetAgent(1);
                main.SetAgentVoice(1);
                main.SetDialogue(1);
                main.checkAgent.text = "Agent: Manaka";
                break;
            default:
                break;
        }
       

        switch (qResult[5])//Cheki
        {
            case "0"://チェキなし
                main.cheki = false;
                main.checkCheki.text = "Cheki: false";
                break;
            case "1"://チェキあり
                main.cheki = true;
                main.checkCheki.text = "Cheki: true";
                break;
            default:
                break;
        }

        switch (str)//mode
        {
            case "deredere":
                print("Mode: Omakase Soda");
                main.mode = 1;
                main.checkMode.text = "Mode: Omakase Soda";
                main.agentAnimator[main.agentNum].SetBool("tsundere", false);
                
                break;
            case "tsundere":
                print("Mode: Tsunderador");
                main.mode = 0;
                main.checkMode.text = "Mode: Tsunderador";
                main.agentAnimator[main.agentNum].SetBool("tsundere", true);

                break;
            default:
                print("Error: the output of 'code.py' is incorrect.");
                break;

        }
    }
    private string PythonCode(string qr0, string qr1, string qr2, string qr3)
    {

        //外部プロセスの設定
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
            FileName = pyExePath, //実行するファイル(python)
            UseShellExecute = false,//シェル機能を使用しない
            CreateNoWindow = true, //コンソール・ウィンドウを開かない
            RedirectStandardOutput = true, //テキスト出力をStandardOutputストリームに書き込むかどうか
            Arguments = pyCodePath + " " + qr0 + " " + qr1 + " " + qr2 + " " + qr3//実行するスクリプト 引数としてアンケート項目の回答結果を入力
        };

        //外部プロセスの開始
        Process process = Process.Start(processStartInfo);

        //ストリームから出力を得る
        StreamReader streamReader = process.StandardOutput;
        string str = streamReader.ReadLine();

        //外部プロセスの終了
        process.WaitForExit();
        process.Close();

        return str;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State_v2
{
    Stanby,
    Start,
    First,
    Second,
    Finish
};

public class StateManager_v2
{
    State_v2 state = State_v2.Stanby;
    public StateManager_v2(string state = "Stanby")
    {
        switch (state)
        {
            case "Stanby":
                this.state = State_v2.Stanby;
                break;
            case "Start":
                this.state = State_v2.Start;
                break;
            case "First":
                this.state = State_v2.First;
                break;
            case "Second":
                this.state = State_v2.Second;
                break;
            case "Finish":
                this.state = State_v2.Finish;
                break;
            default:
                //this.state = State_v2.Stanby;
                break;
        }
    }

    public void nextState()
    {
        switch (this.state)
        {
            case State_v2.Stanby:
                this.state = State_v2.Start;
                break;
            case State_v2.Start:
                this.state = State_v2.First;
                break;
            case State_v2.First:
                this.state = State_v2.Second;
                break;
            case State_v2.Second:
                this.state = State_v2.Finish;
                break;
            case State_v2.Finish:
                this.state = State_v2.Stanby;
                break;
            default:
                //this.state = State_v2.Stanby;
                break;
        }
    }

    public string getState()
    {
        string stateString = "";
        switch (this.state)
        {
            case State_v2.Stanby:
                stateString = "Stanby";
                break;
            case State_v2.Start:
                stateString = "Start";
                break;
            case State_v2.First:
                stateString = "First";
                break;
            case State_v2.Second:
                stateString = "Second";
                break;
            case State_v2.Finish:
                stateString = "Finish";
                break;
            default:
                stateString = "Error";
                break;
        }
        return stateString;
    }
    public void ResetState()
    {
        this.state = State_v2.Stanby;
    }
}

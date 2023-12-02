using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Thx. : https://forum.unity.com/threads/c-proper-state-machine.380612/
/// <summary>
/// CharachterState
/// </summary>
public interface ICharacterState
{
    public void OnEnter();
    public void Execute(float deltaT);
    public void OnExit();
}


/// <summary>
/// StateMachine : use ChangeState<"ICharacterState">() to change state.
/// </summary>
public class StateMachine
{
    private MainCharacter character;
    private List<object> characterListedStates; //TODO: Static to prevent duplication...

    public ICharacterState currentState;


    public StateMachine(MainCharacter character)
    {
        this.character = character;
        InitAllGetCharacterStatesInProject();
    }

    void InitAllGetCharacterStatesInProject()
    {
        characterListedStates = new List<object>();
        List<System.Type> characterListedNames = new List<Type>();

        //Goes trough all scripts in project and filters out those of interface IState.
        System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        foreach (System.Type type in types)
        {
            foreach (System.Type intface in type.GetInterfaces())
            {
                if (intface.Equals(typeof(ICharacterState)))
                {
                    //Add the interface type to list.
                    characterListedNames.Add(type);

                    //Create an instance of the state/class and add to object list.
                    object instance = (object)Activator.CreateInstance(type, new object[] { character });
                    characterListedStates.Add(instance);

#if UNITY_EDITOR
                    string log = "<color=green>StateFound : " + type.FullName.ToString() + "</color>";
                    Debug.Log(log);
#endif
                }
            }
        }

        //Set Init state
        int index = characterListedStates.FindIndex(listState => listState.GetType().Equals(typeof(CC_Walk)));
        if (index == -1)
        {
            Debug.Log("<color=red> State not found! </color>");
            return;
        }
        currentState = characterListedStates[index] as ICharacterState;
        currentState.OnEnter();
    }

    public void Execute(float deltaTime)
    {
        currentState.Execute(deltaTime);
    }

    public void ChangeState<T_State>() where T_State : ICharacterState
    {
        //TODO: Dictionary<type, object>
        int index = characterListedStates.FindIndex(listState => listState.GetType().Equals(typeof(T_State)));
        if (index == -1)
        {
            Debug.Log("<color=red> State not found! </color>");
            return;
        }

        currentState.OnExit();

        currentState = characterListedStates[index] as ICharacterState;
        currentState.OnEnter();

//        Debug.Log(currentState.ToString());
    }

    public void ChangeState(ICharacterState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}

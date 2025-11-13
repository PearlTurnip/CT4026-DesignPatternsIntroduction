using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPattern : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets = null;
    private int currentTarget = 0;

    // History stack for handing undo
    private Stack<ICommand> undoStack;
    private Stack<ICommand> redoStack;


    private void Start()
    {
        undoStack = new Stack<ICommand>();
        redoStack = new Stack<ICommand>();
    }

    private void Update()
    {
        // Add commands if keys are pressed
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Execute(new ForwardCommand());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Execute(new BackwardCommand());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Execute(new LeftCommand());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Execute(new RightCommand());
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Swap();
        }

        // Undo the action if undo is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            Undo();
        }

        // Redo the action if redo is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Redo();
        }
    }

    /// <summary>
    /// Executes a command, adds it to history and includes a target
    /// </summary>
    /// <param name="command"></param>
    private void Execute(ICommand command)
    {
        // Inject target gameobject into the command (if it requires one)
        if (command is TargetCommand)
        {
            (command as TargetCommand).target = targets[currentTarget];
        }

        // Exectute command
        command.Execute();
        // Add it to the undo stack, so we can undo
        undoStack.Push(command);
        // Reset the redo stack
        redoStack.Clear();
    }

    private void Undo()
    {
        // Only if there are items in the stack
        if (undoStack.Count > 0)
        {
            // Get and remove the last item from the stack
            ICommand command = undoStack.Pop();
            // Undo the command
            command.Undo();
            // Add the undone command to the redo stack
            redoStack.Push(command);
        }
    }

    private void Redo() {
        // if there are items on the stack
        if (redoStack.Count > 0) {
            // remove the last item from the stack
            ICommand command = redoStack.Pop();
            // redo the command
            command.Redo();
            // add the redo onto the undo stack
            undoStack.Push(command);
        }
    }

    private void Swap() {
        currentTarget++;
        if (currentTarget >= targets.Length) {
            currentTarget = 0;
        }
    }
}

// ICommand is the basic layout of our commands
// It defines methods that need to be in the class if implemented
// Unlike inheritence, you can implement many interfaces
//
// Eg. One class, extending base class, implementing 2 interfaces:
//
// public class MyClass : BaseClass, IInterface1, IInterface2


public interface ICommand
{

    void Execute();
    void Undo();
    void Redo();
    void Swap();
}

// Target Command stores the target gameboject that we will wish to 
// perform execute and undos upon
// defines two abstract functions for Execute and undo, which child
// classes must implement or pass on if they themselves are abstract
public abstract class TargetCommand : ICommand
{
    // Target for the command to be effected onto
    public GameObject target;

    public abstract void Execute();
    public abstract void Undo();
    public abstract void Redo();
    public abstract void Swap();
}


/// <summary>
/// By adding this middle layer to the inheritance heirarchy, I avoid having
/// to retype all this movement code that's mostly the same between directions.
/// 
/// All that I have to add to each individual direction is the correct vector.
/// </summary>
public abstract class MoveCommand : TargetCommand
{
    //We will need to know where we were prior to moving, in order to undo
    Vector3 oldPosition;
    Vector3 newOldPosition;

    /// <summary>
    /// Store the object we've acted on, and the oldposition, then
    /// move the object by the supplied movement vector
    /// </summary>
    /// <param name="Mobile">GameObject to act upon.</param>
    /// <param name="Movement">Movement vector.</param>
    protected virtual void Move(Vector3 Movement)
    {
        oldPosition = target.transform.position;
        target.transform.position = oldPosition + Movement;
    }

    /// <summary>
    /// This is just passed  on through for the child classes to implement
    /// as this class is also abstract.
    /// </summary>
    public override abstract void Execute();

    /// <summary>
    /// Reverse the action taken
    /// </summary>
    public override void Undo()
    {
        newOldPosition = target.transform.position;
        target.transform.position = oldPosition;
    }

    public override void Redo() {
        target.transform.position = newOldPosition;
    }

    public override void Swap() {

    }
}

// For each of these, because undo is already overridden in MoveCommand,
// they do not need to override that. They have access to "Move" because it
// is in MoveCommand and is protected, so they can call that function in
// their implementation of Execute.

public class ForwardCommand : MoveCommand
{
    public override void Execute()
    {
        Move(new Vector3(0.0f, 0.0f, 1.0f));
    }
}

public class BackwardCommand : MoveCommand
{
    public override void Execute()
    {
        Move(new Vector3(0.0f, 0.0f, -1.0f));
    }
}

public class LeftCommand : MoveCommand
{
    public override void Execute()
    {
        Move(new Vector3(-1.0f, 0.0f, 0.0f));
    }
}

public class RightCommand : MoveCommand
{
    public override void Execute()
    {
        Move(new Vector3(1.0f, 0.0f, 0.0f));
    }
}

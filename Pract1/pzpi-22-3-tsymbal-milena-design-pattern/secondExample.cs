using System;

interface IState
{
    void Handle(AlarmSystem system);
}

class Activated : IState
{
    public void Handle(AlarmSystem system)
    {
        Console.WriteLine("Alarm activated");
        system.SetState(new Alarm());
    }
}

class Alarm : IState
{
    public void Handle(AlarmSystem system)
    {
        Console.WriteLine("Alarm triggered!");
        system.SetState(new Deactivated());
    }
}

class Deactivated : IState
{
    public void Handle(AlarmSystem system)
    {
        Console.WriteLine("Alarm deactivated");
        system.SetState(new Activated());
    }
}

class AlarmSystem
{
    private IState _state;

    public AlarmSystem()
    {
        _state = new Activated();
    }

    public void SetState(IState state)
    {
        _state = state;
    }

    public void Trigger()
    {
        _state.Handle(this);
    }
}

class Program
{
    static void Main()
    {
        var alarmSystem = new AlarmSystem();

        for (int i = 0; i < 6; i++)
        {
            alarmSystem.Trigger();
        }
    }
}

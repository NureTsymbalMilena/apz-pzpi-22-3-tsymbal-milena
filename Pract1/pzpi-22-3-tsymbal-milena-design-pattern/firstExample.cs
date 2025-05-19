using System;

interface IState
{
    void Show();
    void Next(TrafficLight light);
}

class Red : IState
{
    public void Show()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Червоний — стоп!");
        Console.ResetColor();
    }

    public void Next(TrafficLight light)
    {
        light.SetState(new Yellow());
    }
}

class Yellow : IState
{
    public void Show()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Жовтий — уважно!");
        Console.ResetColor();
    }

    public void Next(TrafficLight light)
    {
        light.SetState(new Green());
    }
}

class Green : IState
{
    public void Show()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Зелений — їдь!");
        Console.ResetColor();
    }

    public void Next(TrafficLight light)
    {
        light.SetState(new Red());
    }
}

class TrafficLight
{
    private IState _state;

    public TrafficLight()
    {
        _state = new Red();
    }

    public void SetState(IState state)
    {
        _state = state;
    }

    public void Change()
    {
        _state.Show();
        _state.Next(this);
    }
}

class Program
{
    static void Main()
    {
        var light = new TrafficLight();

        for (int i = 0; i < 9; i++)
        {
            light.Change();
            Thread.Sleep(500);
        }
    }
}
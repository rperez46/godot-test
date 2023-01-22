using Godot;
public static class ColorsCode
{
    public static Color GetColor(int number)
    {
        switch(number)
        {
            case 1:
                return Colors.Red;
            case 2:
                return Colors.Green;
            case 3:
                return Colors.Blue;
            case 4:
                return Colors.DarkOliveGreen;
            case 5:
                return Colors.Purple;
            case 6:
                return Colors.Orange;
            case 7:
                return Colors.DodgerBlue;
        }
        return Colors.White;
    }
}

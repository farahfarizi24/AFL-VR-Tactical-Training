using com.DU.CE.NET.NCM;

public enum EUSERROLE
{
    COACH = 0,
    PLAYER = 1
}


public enum ELVLSTATE
{
    BLANK,

    ONSTART_SETUP,

    ONCONNECT_SETUP,

    ONCONNECT_COACH,
    ONCONNECT_PLAYER,

    SETUP_USER,
    SETUP_USERCOMPONENTS
}


public enum ETEAM
{
    BLANK,
    HOME,
    AWAY
}


public enum EUSERSTATE
{
    BLANK,
    DEFAULT,

    SETUP_COMPONENTS,

    PLAYREADY,
}

public enum EUSERHAND
{
    LEFT = 1,
    RIGHT = -1,

    // Mat.Abs(Left(1) - Right(-1)) = 2
    NONE = 2,

    // Left(1) + Right(-1) = 0
    BOTH = 0
}

public enum EINTERACTIONSTATE
{
    Blank,
    // % 10 = Action
    // (int)( * 0.1) = Interactor
    
    ONGRAB = 11,
    ONRELEASE = 12,

    ONSELECT = 21,
    ONDESELECT = 22
}

public struct HandInfo
{
    public EUSERHAND Hand;
    public EINTERACTIONSTATE InteractorState;
}


public enum EBRUSH
{
    BOARD,
    PATH
}

public struct BrushStrokeInfo
{
    public EBRUSH BrushType;
    public NET_BrushStroke Stroke;
}
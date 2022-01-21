[System.Serializable]
public class SaveData
{
    //Time in minutes that this save slot has on file
    private float _playTime;
    public float PlayTime
    {
        get { return _playTime; }
        set { _playTime = value; }
    }

    //Index of the furthest level reached by the player
    private int _furthestLevel;
    public int FurthestLevel
    {
        get { return _furthestLevel; }
        set { _furthestLevel = value; }
    }

    //Constructor to create a new SaveData class
    public SaveData(float playTime, int furthestLevel)
    {
        this._playTime = playTime;
        this._furthestLevel = furthestLevel;
    }
}

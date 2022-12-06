public enum Role
{
    rabbit, deer, swan, cat, wolf, empty
}

public class Player
{
    public int id;
    public string name;
    public Role role = Role.empty;

    public bool isAlive = true;
    public Player selected = null;
}

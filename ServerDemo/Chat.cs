using System.Collections.Generic;

public class Chat
{
    public List<User> users;
    public List<Channel> channels;

    public Chat()
    {
        users = new List<User>();
        channels = new List<Channel>();
    }
}
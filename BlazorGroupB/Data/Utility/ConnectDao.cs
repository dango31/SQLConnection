using BlazorGroupB.Data.DAO;
using Npgsql;

namespace BlazorGroupB.Data.Utility;

public class ConnectDao
{
    //  各種Daoクラス。インスタンスは後で行う
    private NpgsqlConnection conn = null;
    public ThreadsDao threadsDao;
    public MessagesDao messagesDao;
    public UsersDao usersDao;

    public ConnectDao()
    {
        try
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                //Database = "test",
                Database = "GroupB_DataBase",
                Username = "postgres",
                Password = "postgres"
            };

            conn = new NpgsqlConnection(builder.ConnectionString);

            threadsDao = new ThreadsDao(conn);
            messagesDao = new MessagesDao(conn);
            usersDao = new UsersDao(conn);
        }
        catch
        {
            throw;
        }

    }
}

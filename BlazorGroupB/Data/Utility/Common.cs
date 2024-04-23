using BlazorGroupB.Models;

namespace BlazorGroupB.Data.Utility;

public class Common
{
    //  Daoとの接続
    private ConnectDao connectDao;

    public Common()
    {
        connectDao = new ConnectDao();
    }

    /// <summary>
    /// Daoを使ってメッセージを作成する
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="number"></param>
    /// <param name="userdataID"></param>
    /// <exception cref="Exception"></exception>
    public void PostMessage(Messages msg, int number, string userdataID)
    {
        try
        {
            //  ThreadIDを追加する
            msg.ThreadID = number;

            //  メッセージにUserIDを打ち込む
            msg.UserID = userdataID;

            //  メッセージに時間を打ち込む
            DateTime dt = DateTime.Now;
            msg.PostTime = dt.ToString();

            //  メッセージを作成する
            connectDao.messagesDao.Insert(msg);

            //  投稿時間をスレッドにUPDATEする
            connectDao.threadsDao.Update(dt, number);

        }
        catch
        {
            throw new Exception("PostMessagesエラー");

        }

    }

    /// <summary>
    /// Daoを使ってスレッドを作成する
    /// </summary>
    /// <param name="th"></param>
    /// <param name="userdataID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int NewThread(Threads th, string userdataID)
    {
        try
        {
            //  作成時刻を追加する
            DateTime dt = DateTime.Now;
            th.ThreadCreateDate = dt.ToString();

            //  ユーザーIDを追加する
            th.UserID = userdataID;

            //  スレッドを作成する
            int number = connectDao.threadsDao.Insert(th);

            return number;
        }
        catch
        {
            throw new Exception("NewThreadエラー");
        }

    }

    /// <summary>
    /// Daoを使ってUserを作成する
    /// </summary>
    /// <param name="userAgent"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string CreateUserID(string userAgent)
    {
        try
        {
            Users user = new Users();
            user.UserAgent = userAgent;
            user.UserCreateDate = DateTime.Now;

            //  Userを作成する
            return connectDao.usersDao.Insert(user);
        }
        catch
        {
            throw new Exception("CreateUserIDエラー");
        }

    }


}

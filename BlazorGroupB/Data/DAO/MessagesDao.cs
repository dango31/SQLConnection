using BlazorGroupB.Models;
using BlazorGroupB.Pages.bbs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace BlazorGroupB.Data.DAO;

public class MessagesDao
{
    private NpgsqlConnection conn;
    public MessagesDao(NpgsqlConnection connection)
    {
        conn = connection;
    }

    public bool Connection()
    {
        //  接続の確認
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            Debug.WriteLine("接続成功");
            return true;

        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("Messagesコネクションオープンエラー");

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("Messagesコネクション処理エラー");

        }

    }

    public int Insert(Messages msg)
    {
        NpgsqlTransaction transaction = null;

        //  接続確認
        if (!Connection())
            throw new Exception("NpgSqlの接続に失敗しました");


        //  DTOの確認
        if (msg == null)
            throw new Exception("新規メッセージ情報の取得に失敗しました");

        try
        {
            //  INSERT文
            //  アカウントの部分だけ編集を行う

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO \"Messages\"(")
                .Append(" \"ThreadID\", \"UserID\", \"WriteName\", \"DetailMessage\", \"PostTime\" )")
                .Append("VALUES( ")
                .Append(" @thread_id, @user_id, @write_name, @detail_message, @post_time")
                .Append(") RETURNING \"ThreadID\"");

            //  書き込みを行う
            NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

            //  パラメーターのセット
            cmd.Parameters.AddWithValue("@thread_id", msg.ThreadID);
            cmd.Parameters.AddWithValue("@user_id", msg.UserID ?? "UserIDの取得に失敗");
            cmd.Parameters.AddWithValue("@write_name", msg.WriteName ?? "名無し");
            cmd.Parameters.AddWithValue("@detail_message", msg.DetailMessage ?? "本文の取得に失敗");
            cmd.Parameters.AddWithValue("@post_time", msg.PostTime);

            //  TRANSACTION
            transaction = conn.BeginTransaction();
            int lastid = (int)cmd.ExecuteScalar();

            //  TRANSACTIONコミット
            transaction.Commit();

            return lastid;
        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("Messagesインサートエラー");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("Messagesインサート処理エラー");
        }
        finally
        {
            conn?.Close();
        }

    }


}

﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bacchus.Model;
namespace Bacchus
{
    class DBManager
    {
        public static int nextRefFamille = 1;
        public static int nextRefSousFamille = 1;
        public static int nextRefMarque = 1;
        public static int insertFamille(Famille famille)
        {
            int inserted = 0;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES(@Ref, @Nom)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@Ref", nextRefFamille);
            cmd.Parameters.AddWithValue("@Nom", famille.Nom);
            try
            {
                inserted = cmd.ExecuteNonQuery();
                if (inserted > 0) famille.RefFamille=nextRefFamille++;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            return inserted;
        }


        public static int insertSousFamille(SousFamille sousFamille)
        {
            int inserted = 0;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "INSERT INTO SousFamilles(RefSousFamille,RefFamille, Nom) VALUES(@Ref,@RefFamille, @Nom)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@Ref", nextRefSousFamille);
            cmd.Parameters.AddWithValue("@RefFamille", sousFamille.Famille.RefFamille);
            cmd.Parameters.AddWithValue("@Nom", sousFamille.Nom);
            try
            {
                inserted = cmd.ExecuteNonQuery();
                if (inserted > 0) sousFamille.RefSousFamille=nextRefSousFamille++;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            return inserted;
        }

        public static int insertMarque(Marque marque)
        {
            int inserted = 0;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "INSERT INTO Marques(RefMarque, Nom) VALUES(@Ref, @Nom)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@Ref", nextRefMarque);
            cmd.Parameters.AddWithValue("@Nom", marque.Nom);
            try
            {
                inserted = cmd.ExecuteNonQuery();
                if (inserted > 0) marque.RefMarque = nextRefMarque++;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            return inserted;
        }

        public static int WIPEOUT()
        {
            int rowDeleted=0;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "DELETE FROM Marques";
            rowDeleted += cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM Articles";
            rowDeleted += cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM Familles";
            rowDeleted += cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM SousFamilles";
            rowDeleted += cmd.ExecuteNonQuery();

            return rowDeleted;
        }

        public static Famille getFamilleByRef(int refFamille)
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM Familles WHERE RefFamille = @Ref";
            cmd.Parameters.AddWithValue("@Ref", refFamille);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                Famille famille = null;
                while (reader.Read())
                {
                    famille = new Famille();
                    famille.RefFamille = reader.GetInt32(0);
                    famille.Nom = reader.GetString(1);

                }
                return famille;
            }
        }

        public static SousFamille getSousFamilleByRef(int refSousFamille) //TODO : TEST
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM SousFamilles WHERE RefSousFamille = @ref";
            cmd.Parameters.AddWithValue("@ref", refSousFamille);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                SousFamille sousfamille = null;
                while (reader.Read())
                {
                    Famille famille = DBManager.getFamilleByRef(reader.GetInt32(1));
                    sousfamille = new SousFamille();
                    sousfamille.RefSousFamille = reader.GetInt32(0);
                    sousfamille.Famille = famille;
                    sousfamille.Nom = reader.GetString(2);

                }
                return sousfamille;
            }
        }

        public static Famille getFamilleByName(string nomFamille)
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM Familles WHERE Nom = @Nom";
            cmd.Parameters.AddWithValue("@Nom", nomFamille);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                Famille famille = null;
                while (reader.Read())
                {
                     famille = new Famille();
                    famille.RefFamille = reader.GetInt32(0);
                    famille.Nom = reader.GetString(1);
                    
                }
                return famille;
            }
        }

        public static SousFamille getSousFamilleByName(string nomSousFamille) //TODO : TEST
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM SousFamilles WHERE Nom = @Nom";
            cmd.Parameters.AddWithValue("@Nom", nomSousFamille);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                SousFamille sousfamille = null;
                while (reader.Read())
                {
                    Famille famille = DBManager.getFamilleByRef(reader.GetInt32(1));


                        sousfamille = new SousFamille();
               
                    sousfamille.RefSousFamille = reader.GetInt32(0);
                    sousfamille.Famille = famille;
                    sousfamille.Nom = reader.GetString(2);

                }
                return sousfamille;
            }
        }

        public static Marque getMarqueByName(string nomMarque)
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM Marques WHERE Nom = @Nom";
            cmd.Parameters.AddWithValue("@Nom", nomMarque);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                Marque marque = null;
                while (reader.Read())
                {
                    marque = new Marque();
                    marque.RefMarque = reader.GetInt32(0);
                    marque.Nom = reader.GetString(1);

                }
                return marque;
            }
        }

        public static List<Article> getAllArticles()
        {
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT * FROM Articles A NATURAL JOIN Marques M JOIN SousFamilles SF ON A.RefSousFamille= SF.RefSousFamille  JOIN Familles F ON SF.RefFamille = F.RefFamille;";
            List<Article> articles = new List<Article>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                Marque marque = null;
                Famille famille = null;
                SousFamille sousFamille = null;
                Article article = null;

                
                while (reader.Read())
                {
                    marque = new Marque();
                    famille = new Famille();
                    sousFamille = new SousFamille();
                    article = new Article();
                    article.RefArticle = reader.GetString(0);
                    article.Description = reader.GetString(1);
                    sousFamille.RefSousFamille = reader.GetInt32(2);
                    marque.RefMarque = reader.GetInt32(3);
                    article.PrixHT = reader.GetFloat(4);
                    marque.Nom = reader.GetString(6);
                    famille.RefFamille = reader.GetInt32(8);
                    sousFamille.Nom = reader.GetString(9);
                    famille.Nom = reader.GetString(11);
                    sousFamille.Famille = famille;
                    article.SousFamille = sousFamille;
                    article.Marque = marque;
                    articles.Add(article);


                }
                return articles;
            }
        }

        public static bool insertArticle(Article article)
        {
            bool inserted = true;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES(@Ref,@Desc,@RefSousFamille,@RefMarque,@Prix,@Qt); ";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@Ref", article.RefArticle);
            cmd.Parameters.AddWithValue("@Desc", article.Description);
            cmd.Parameters.AddWithValue("@RefSousFamille", article.SousFamille.RefSousFamille);
            cmd.Parameters.AddWithValue("@RefMarque", article.Marque.RefMarque);
            cmd.Parameters.AddWithValue("@Prix", article.PrixHT);
            cmd.Parameters.AddWithValue("@Qt", article.Quantite);
            try
            {
               cmd.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                inserted = false;
            }
            return inserted;
        }

        public static void MajRef()
        {
            int nextID;
            SQLiteCommand cmd = new SQLiteCommand(DBConnection.Instance);
            cmd.CommandText = "SELECT  MAX(RefFamille) AS nextID  FROM Familles";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["nextID"].ToString() != "")
                    {
                        nextID = Int32.Parse(reader["nextID"].ToString());
                        nextRefFamille = ++nextID;
                    }
                    else
                    {
                        nextRefFamille = 1;
                    }
                }
            }
            cmd.CommandText = "SELECT  MAX(RefMarque) AS nextID  FROM Marques";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["nextID"].ToString() != "")
                    {
                        nextID = Int32.Parse(reader["nextID"].ToString());
                        nextRefMarque = ++nextID;
                    }
                    else
                    {
                        nextRefMarque = 1;
                    }
                }
            }
            cmd.CommandText = "SELECT  MAX(RefSousFamille) AS nextID  FROM SousFamilles";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["nextID"].ToString() != "")
                    {
                        nextID = Int32.Parse(reader["nextID"].ToString());
                        nextRefSousFamille = ++nextID;
                    }
                    else
                    {
                        nextRefSousFamille = 1;
                    }
                }
            }


        }
    } 
        
}


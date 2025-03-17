
using MuscicCenter.Storage;
using System.Data;
using System.Data.SqlClient;

namespace MusicCenterAPI.ProcedureStorage
{
    public class ProcedureManager
    {
        MusicCenterAPI _api;
        public ProcedureManager(MusicCenterAPI api)
        {
            _api = api;
        }
        public void createProcedure()
        {
            _api.ProcedureCreate("getPlaylistByUserName", @"CREATE PROCEDURE getPlaylistByUserName
                                                     @UserName NVARCHAR(20)
                                                     AS
                                                     BEGIN
                                                         SELECT * 
                                                         FROM Playlist
                                                         WHERE UserName = @UserName;
                                                     END");

            _api.ProcedureCreate("getInfoPlaylistByUid", @"CREATE PROCEDURE getInfoPlaylistByUid
                                                     @Uid NVARCHAR(50)
                                                     AS
                                                     BEGIN
                                                         SELECT * 
                                                         FROM InfoPlaylist
                                                         WHERE PlaylistUid = @Uid;
                                                     END");
            _api.ProcedureCreate("getAmountRecordInPlaylistByUid", @"CREATE PROCEDURE getAmountRecordInPlaylistByUid
                                                     @Uid NVARCHAR(50)
                                                     AS
                                                     BEGIN
                                                         SELECT count(*) AS Amount
                                                         FROM InfoPlaylist
                                                         WHERE PlaylistUid = @Uid;
                                                     END");
            _api.ProcedureCreate("getRecordUidsFromPlaylist", @"CREATE PROCEDURE getRecordUidsFromPlaylist
                                                     @PlaylistUid NVARCHAR(50)
                                                     AS
                                                     BEGIN
                                                         SELECT RecordUid
                                                         FROM InfoPlaylist
                                                         WHERE PlaylistUid = @PlaylistUid;
                                                     END");
            _api.ProcedureCreate("getRecordsFromPlaylist", @"CREATE PROCEDURE getRecordsFromPlaylist
                                                     @PlaylistUid NVARCHAR(50)
                                                     AS
                                                     BEGIN
                                                        SELECT R.*, A.StageName, A.Avata, A.Visits, C.DisplayName AS CategoryName
                                                        FROM Record R
                                                        INNER JOIN InfoPlaylist IP ON R.RecordUid = IP.RecordUid
                                                        INNER JOIN Artist A On R.ArtistUid = A.ArtistUid
                                                        INNER JOIN Category C On R.CategoryUid = C.CategoryUid
                                                        WHERE IP.PlaylistUid = @PlaylistUid;
                                                     END");
            _api.ProcedureCreate("removeRecordInPlaylist", $@"CREATE PROCEDURE removeRecordInPlaylist
                                                     @PlaylistUid NVARCHAR(50),
                                                     @RecordUid NVARCHAR(50)
                                                     AS
                                                     BEGIN
                                                         DELETE {DatabaseStruct.InfoPlaylistTable} WHERE PlaylistUid = @PlaylistUid AND RecordUid = @RecordUid;
                                                     END");
            _api.ProcedureCreate("getAllArtists", @"CREATE PROCEDURE getAllArtists
                                                     AS
                                                     BEGIN
                                                         SELECT * FROM Artist;
                                                     END");
            _api.ProcedureCreate("getAllCategory", @"CREATE PROCEDURE getAllCategory
                                                     AS
                                                     BEGIN
                                                         SELECT * FROM Category;
                                                     END");
            _api.ProcedureCreate("getAllRecord", @"CREATE PROCEDURE getAllRecord
                                                     AS
                                                     BEGIN
                                                        SELECT R.*, A.StageName,A.Visits, A.Avata, C.DisplayName AS CategoryName
                                                        FROM Record R
                                                        INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                        INNER JOIN Category C ON C.CategoryUid = R.CategoryUid;
                                                     END");
            _api.ProcedureCreate("getTopRecordByView", @"CREATE PROCEDURE getTopRecordByView
                                                    @TopRows INT
                                                     AS
                                                     BEGIN
                                                        SELECT TOP (@TopRows) *, A.StageName,A.Visits, A.Avata, C.DisplayName AS CategoryName
                                                        FROM Record R
                                                        INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                        INNER JOIN Category C ON C.CategoryUid = R.CategoryUid
                                                        ORDER BY Views DESC;
                                                     END");
            _api.ProcedureCreate("getNewRecords", @"CREATE PROCEDURE getNewRecords
                                                    @TopRows INT
                                                     AS
                                                     BEGIN
                                                        SELECT TOP (@TopRows) *, A.StageName,A.Visits, A.Avata, C.DisplayName AS CategoryName
                                                        FROM Record R
                                                        INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                        INNER JOIN Category C ON C.CategoryUid = R.CategoryUid;
                                                     END");
            _api.ProcedureCreate(ProcedureName.GET_RECORDS_FOR_ARTIST.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_RECORDS_FOR_ARTIST.GetValue()} 
                                                        @ArtistUid NVARCHAR(50)
                                                         AS
                                                         BEGIN
                                                            SELECT R.*, A.StageName,A.Visits, A.Avata, C.DisplayName AS CategoryName
                                                            FROM Record R
                                                            INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                            INNER JOIN Category C ON C.CategoryUid = R.CategoryUid
                                                            WHERE A.ArtistUid = @ArtistUid;
                                                        END");
            _api.ProcedureCreate("recordSearchByKeyWord", @"CREATE PROCEDURE recordSearchByKeyWord 
                                                        @keyword NVARCHAR(50),
                                                        @Page INT
                                                        AS
                                                        BEGIN
                                                            SELECT R.*, A.StageName, A.Avata,A.Visits, C.DisplayName As CategoryName
                                                            FROM Record R 
                                                            INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                            INNER JOIN Category C On C.CategoryUid = R.CategoryUid
                                                            WHERE LOWER(R.RecordUid) LIKE ''%'' + LOWER(@keyword) + ''%''
                                                               OR LOWER(R.DisplayName) LIKE ''%'' + LOWER(@keyword) + ''%''
                                                            ORDER BY R.Views DESC
                                                            OFFSET (@Page - 1) * 5 ROWS
                                                            FETCH NEXT 5 ROWS ONLY;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_RECORDS_BY_CATEGORYUID.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_RECORDS_BY_CATEGORYUID.GetValue()} 
                                                        @CategoryUid NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            SELECT R.*, A.StageName, A.Avata,A.Visits, C.DisplayName As CategoryName
                                                            FROM Record R 
                                                            INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                            INNER JOIN Category C On C.CategoryUid = R.CategoryUid
                                                            WHERE R.CategoryUid = @CategoryUid ORDER BY Views DESC;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_COMMENTS_BY_RECORDUID.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_COMMENTS_BY_RECORDUID.GetValue()} 
                                                        @RecordUid NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            SELECT c.*, COUNT(r.CommentUid) AS totalReact
                                                            FROM Comments c
                                                            LEFT JOIN ReactComment r ON c.CommentUid = r.CommentUid
                                                            Where c.RecordUid = @RecordUid
                                                            GROUP BY c.CommentUid, c.UserName, c.Content, c.RecordUid, c.TimeOfComment
                                                            ORDER BY totalReact DESC;
                                                        END");
            _api.ProcedureCreate(ProcedureName.ADD_VISIT_BY_ARTISTUID.GetValue(), $@"CREATE PROCEDURE {ProcedureName.ADD_VISIT_BY_ARTISTUID.GetValue()} 
                                                        @Artistuid NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            update Artist set visits = visits + 1 where ArtistUid = @ArtistUid;
                                                        END");
            _api.ProcedureCreate(ProcedureName.REMOVE_RECORD_FROM_FAVORITE.GetValue(), $@"CREATE PROCEDURE {ProcedureName.REMOVE_RECORD_FROM_FAVORITE.GetValue()} 
                                                        @UserName NVARCHAR(50),
                                                        @RecordUid NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            DELETE Favourite WHERE UserName = @UserName AND RecordUid = @RecordUid;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_FAVORITELIST_BY_USERNAME.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_FAVORITELIST_BY_USERNAME.GetValue()} 
                                                        @UserName NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            select r.*, a.StageName, c.DisplayName as cate_display from Record r
                                                            JOIN Artist a On a.ArtistUid = r.ArtistUid
                                                            JOIN Category c On c.CategoryUid = r.CategoryUid
                                                            Inner join Favourite f ON f.RecordUid = r.RecordUid
                                                            WHERE f.UserName = @UserName;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_RECORD_BY_PAGE.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_RECORD_BY_PAGE.GetValue()} 
                                                        @Page INT,
                                                        @Rows INT
                                                        AS
                                                        BEGIN
                                                            SELECT R.*, A.StageName, A.Avata,A.Visits, C.DisplayName As CategoryName
                                                            FROM Record R 
                                                            INNER JOIN Artist A ON A.ArtistUid = R.ArtistUid
                                                            INNER JOIN Category C On C.CategoryUid = R.CategoryUid
                                                            ORDER BY Views DESC
                                                            OFFSET (@Page - 1) * @Rows ROWS
                                                            FETCH NEXT @Rows ROWS ONLY;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_ARTISTS_BY_PAGE.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_ARTISTS_BY_PAGE.GetValue()} 
                                                        @Page INT,
                                                        @Rows INT
                                                        AS
                                                        BEGIN
                                                            SELECT
                                                            CASE 
                                                                WHEN CAST(SUM(R.Views) as INT) IS NULL THEN 0
                                                                ELSE SUM(R.Views)
                                                            END AS TotalView,
                                                            A.*, A.Visits AS TotalVisits, COUNT(R.RecordUid) AS TotalRecord 
                                                            FROM Artist A
                                                            LEFT JOIN Record R On R.ArtistUid = A.ArtistUid 
                                                            GROUP BY A.ArtistUid, A.Avata, A.StageName, A.Visits
                                                            ORDER BY A.Visits DESC
                                                            OFFSET(@Page - 1) * @Rows ROWS
                                                            FETCH NEXT @Rows ROWS ONLY;
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_ACCOUNTS_BY_PAGE.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_ACCOUNTS_BY_PAGE.GetValue()} 
                                                        @Page INT
                                                        AS
                                                        BEGIN
                                                            SELECT * FROM Account A
                                                            ORDER BY JoinDay DESC
                                                            OFFSET(@Page - 1) * 10 ROWS
                                                            FETCH NEXT 10 ROWS ONLY;
                                                        END");
            _api.ProcedureCreate(ProcedureName.ADD_TO_FAVORITELIST.GetValue(), $@"CREATE PROCEDURE {ProcedureName.ADD_TO_FAVORITELIST.GetValue()} 
                                                        @UserName NVARCHAR(50),
                                                        @RecordUid NVARCHAR(50)
                                                        AS
                                                        BEGIN
                                                            IF NOT EXISTS (
                                                                SELECT 1 FROM Favourite WHERE UserName = @UserName AND RecordUid = @RecordUid
                                                            )
                                                            BEGIN
	                                                            INSERT INTO Favourite (UserName, RecordUid)
	                                                            VALUES (@UserName, @RecordUid)
                                                            END
                                                        END");
            _api.ProcedureCreate(ProcedureName.GET_TOP_CATEGORY.GetValue(), $@"CREATE PROCEDURE {ProcedureName.GET_TOP_CATEGORY.GetValue()} 
                                                        AS
                                                        BEGIN
                                                            WITH RankedRecords AS (
                                                                SELECT r.CategoryUid,
                                                                       r.Poster,
                                                                       r.Views,
                                                                       ROW_NUMBER() OVER (PARTITION BY r.CategoryUid ORDER BY r.Views DESC) AS RowNum
                                                                FROM Record r
                                                            )
                                                            SELECT c.*,
                                                                   COUNT(r.RecordUid) AS TotalRecords,
                                                                   SUM(r.Views) AS TotalViews,
                                                                   rr.Poster AS OutstandingRecordPoster
                                                            FROM Category c
                                                            JOIN Record r ON c.CategoryUid = r.CategoryUid
                                                            JOIN RankedRecords rr ON rr.CategoryUid = c.CategoryUid AND rr.RowNum = 1
                                                            GROUP BY c.CategoryUid, c.DisplayName, rr.Poster
                                                            ORDER BY TotalViews DESC;
                                                        END");
            _api.ProcedureCreate(ProcedureName.LOGIN.GetValue(), $@"CREATE PROCEDURE {ProcedureName.LOGIN.GetValue()}
                                                            @username NVARCHAR(max),
                                                            @hashpass VARCHAR(MAX)
                                                        AS
                                                        BEGIN
                                                            SELECT * FROM Account WHERE UserName = @username AND Password = @hashpass
                                                        END");
        }
        public DataTable ProcedureCall(ProcedureName name, SqlParameter[]? parameters)
        {
            return _api.ProcedureCall(name.GetValue(), parameters);
        }
    }
}

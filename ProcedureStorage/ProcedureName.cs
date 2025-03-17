using System.ComponentModel.DataAnnotations;

namespace MusicCenterAPI.ProcedureStorage
{
    public enum ProcedureName
    {

        GET_PLAYLISTS_BY_USER_NAME,
        GET_INFO_PLAYLIST_BY_UID,
        GET_AMOUNT_RECORD_IN_PLAYLIST_BY_UID,
        GET_RECORD_UIDS_FROM_PLAYLIST,
        GET_RECORDS_FROM_PLAYLIST,
        REMOVE_RECORD_IN_PLAYLIST,
        GET_ALL_ARTISTS,
        GET_ALL_RECORDS,
        GET_ALL_CATEGORY,
        GET_TOP_RECORD_BY_VIEW,
        GET_NEW_RECORDS,
        GET_RECORDS_FOR_ARTIST,
        RECORD_SEARCH_BY_KEYWORD,
        GET_TOP_CATEGORY,
        GET_RECORDS_BY_CATEGORYUID,
        GET_COMMENTS_BY_RECORDUID,
        ADD_VISIT_BY_ARTISTUID,
        ADD_TO_FAVORITELIST,
        GET_FAVORITELIST_BY_USERNAME,
        REMOVE_RECORD_FROM_FAVORITE,
        GET_RECORD_BY_PAGE,
        GET_ACCOUNTS_BY_PAGE,
        LOGIN,
        GET_ARTISTS_BY_PAGE

    }
    public static class ProcedureExtensions
    {
        public static string GetValue(this ProcedureName value)
        {
            switch (value)
            {
                case ProcedureName.GET_PLAYLISTS_BY_USER_NAME:
                    return "getPlaylistByUserName";
                case ProcedureName.GET_INFO_PLAYLIST_BY_UID:
                    return "getInfoPlaylistByUid";
                case ProcedureName.GET_AMOUNT_RECORD_IN_PLAYLIST_BY_UID:
                    return "getAmountRecordInPlaylistByUid";
                case ProcedureName.GET_RECORD_UIDS_FROM_PLAYLIST:
                    return "getRecordUidsFromPlaylist";
                case ProcedureName.GET_RECORDS_FROM_PLAYLIST:
                    return "getRecordsFromPlaylist";
                case ProcedureName.REMOVE_RECORD_IN_PLAYLIST:
                    return "removeRecordInPlaylist";
                case ProcedureName.GET_ALL_ARTISTS:
                    return "getAllArtists";
                case ProcedureName.GET_ALL_CATEGORY:
                    return "getAllCategory";
                case ProcedureName.GET_ALL_RECORDS:
                    return "getAllRecord";
                case ProcedureName.GET_TOP_RECORD_BY_VIEW:
                    return "getTopRecordByView";
                case ProcedureName.GET_NEW_RECORDS:
                    return "getNewRecords";
                case ProcedureName.GET_RECORDS_FOR_ARTIST:
                    return "getRecordsForArtist";
                case ProcedureName.RECORD_SEARCH_BY_KEYWORD:
                    return "recordSearchByKeyWord";
                case ProcedureName.GET_TOP_CATEGORY:
                    return "getTopCategory";
                case ProcedureName.GET_RECORDS_BY_CATEGORYUID:
                    return "getRecordsByCategoryUid";
                case ProcedureName.GET_COMMENTS_BY_RECORDUID:
                    return "getCommentsByRecordUid";
                case ProcedureName.ADD_VISIT_BY_ARTISTUID:
                    return "addVisitByArtistUid";
                case ProcedureName.ADD_TO_FAVORITELIST:
                    return "addToFavoriteList";
                case ProcedureName.GET_FAVORITELIST_BY_USERNAME:
                    return "getFavoriteListByUserName";
                case ProcedureName.REMOVE_RECORD_FROM_FAVORITE:
                    return "removeRecordFromFavoriteList";
                case ProcedureName.GET_RECORD_BY_PAGE:
                    return "getRecordByPage";
                case ProcedureName.GET_ARTISTS_BY_PAGE:
                    return "getArtistsByPage";
                case ProcedureName.GET_ACCOUNTS_BY_PAGE:
                    return "getAccountsByPage";
                case ProcedureName.LOGIN:
                    return "login";
                default:
                    return value.ToString();
            }
        }
    }
}

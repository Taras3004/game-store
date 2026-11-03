namespace GameStore.Core.Specifications.BanRecords
{
    internal class ActiveBanRecordByUsernameSpecification(string username, DateTime currentDateTime)
        : BanRecordSpecificationBase(record => record.Username == username
                                               && record.BannedUntil > currentDateTime);
}
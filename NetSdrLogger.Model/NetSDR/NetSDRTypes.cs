namespace NetSdrLogger.Model.NetSDR
{
    internal enum HostMessageType
    {
        HostSetControlItem = 0, //000
        HostRequestCurrentControlItem = 1,  //001
        HostRequestControlItemRange = 2, //010
        HostDataItemACKfromHosttoTarget = 3, //011
        HostHostDataItem0 = 4, //100
        HostHostDataItem1 = 5, //101
        HostHostDataItem2 = 6, //110
        HostHostDataItem3 = 7, //111
    }
    internal enum TargetMessageType
    {
        TargetResponsetoSetorRequestCurrentControlItem = 0, //000
        TargetUnsolicitedControlItem = 1, //001
        TargetResponsetoRequestControlItemRange = 2, //010
        TargetDataItemACKfromTargettoHost = 3, //011
        TargetTargetDataItem0 = 4, //100
        TargetTargetDataItem1 = 5, //101
        TargetTargetDataItem2 = 6, //110
        TargetTargetDataItem3 = 7 //111
    }
}

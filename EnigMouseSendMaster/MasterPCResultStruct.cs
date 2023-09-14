using MessagePack;

namespace EnigMouseSendMaster
{
    [MessagePackObject]
    [Serializable]
    public struct MasterPCResultStruct
    {
        [Key(0)]
        public string IPAddress;
        [Key(1)]
        public List<ResultStruct> ResultStructs;

        public MasterPCResultStruct(string ipAddress,List<ResultStruct> resultStructs)
        {
            IPAddress = ipAddress;
            ResultStructs = resultStructs;
        }
    }
}

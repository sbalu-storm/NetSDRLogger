namespace NetSdrLogger.Model
{
    //byte sequence correctly translated to header and dataitem
    //dataitem correctly translated to a single_collection

    //two dataitems correctly translated to a single_collection
    //two dataitems correctly translated to two collections
    //three dataitems correctly translated to two collections

    //collection aggregation works fine: 
    //in frequence scope, out of frequency scope

    // average frequency, other avg calculated correctly

    // average frequency handled correctly: averagefreq is the bounds to agregate new signal or not

    //Unix?DateTime is handled correctly

    //TCP socket mocked stream is handled correctly
    // for one message
    // for several messages : 
    //      two good messages
    //      bad,two good,bad,good messages
    //      good with error message
    // for one data item
    // for several data items

    public class CollectionServiceTests
    {
        [Fact]
        public void Test1()
        {

        }
    }
    public class TCPSignalSourceTests
    {
        [Fact]
        public void Test1()
        {

        }
    }
    public class HardwareHeaderTests
    {
        [Fact]
        public void Test1()
        {

        }
    }
    public class SignalTransmittion
    {
        [Fact]
        public void Test1()
        {

        }
    }
}
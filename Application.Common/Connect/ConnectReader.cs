namespace ExecutionEngine.Common.Connect
{
	public interface ConnectReader
	{
	  string read();
	  sbyte[] readByte();
	  void clear();
	  bool hasContent();
	}
}
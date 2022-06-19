using System;
namespace roslynTester
{
	public class Value
	{
		public object value;
		public bool display;
        public string dataType;

		public Value(object value, bool display, string typeInfo = "")
        {
            this.value = value;
			this.display = display;
            this.dataType = typeInfo;
        }

        public override string ToString()
        {
            if (!this.display)
            {
                return "";
            }
            return value.ToString();
        }

    }
}


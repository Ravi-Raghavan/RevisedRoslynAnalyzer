using System;
namespace roslynTester
{
	public class Value
	{
		public object value;
		public bool display;

		public Value(object value, bool display)
        {
            this.value = value;
			this.display = display;
        }

        public override string ToString()
        {
            return value.ToString();
        }

    }
}


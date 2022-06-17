using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
namespace roslynTester
{
	public class Evaluate
	{
		//Evaluates Arithmetic Expressions
		public static async Task<string> evaluateExpression(string expression, Dictionary<string, object> currentValues)
        {
			if(currentValues.Count == 0)
            {
				var finalValue = await CSharpScript.RunAsync(expression);
				return finalValue.ReturnValue.ToString();

			}
			int index = 0;
            var value = await CSharpScript.RunAsync($"int {currentValues.ElementAt(0).Key} = {currentValues.ElementAt(0).Value};");
			for(int i = 1; i < currentValues.Count; i++)
            {
				KeyValuePair<string, object> keyValuePair = currentValues.ElementAt(i);
				value = await value.ContinueWithAsync($"int {keyValuePair.Key} = {keyValuePair.Value};");
			}

			value = await value.ContinueWithAsync(expression);

			return value.ReturnValue.ToString();
        }

		public static async Task<string> evaluateFunction(string functionString, string expression, Dictionary<string, object> currentValues)
        {
			//Console.WriteLine("Function Code: " + functionString);
			var finalValue = await CSharpScript.RunAsync(functionString);
			if (currentValues.Count == 0)
			{
				finalValue = await finalValue.ContinueWithAsync(expression);
				return finalValue.ReturnValue.ToString();

			}
			int index = 0;
			finalValue = await finalValue.ContinueWithAsync($"int {currentValues.ElementAt(0).Key} = {currentValues.ElementAt(0).Value};");
			for (int i = 1; i < currentValues.Count; i++)
			{
				KeyValuePair<string, object> keyValuePair = currentValues.ElementAt(i);
				finalValue = await finalValue.ContinueWithAsync($"int {keyValuePair.Key} = {keyValuePair.Value};");
			}

			finalValue = await finalValue.ContinueWithAsync(expression);

			return finalValue.ReturnValue.ToString();

		}
	}
}


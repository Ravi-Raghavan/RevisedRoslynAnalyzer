using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
namespace roslynTester
{
	public class Evaluate
	{
		//Evaluates Arithmetic Expressions
		public static async Task<string> evaluateExpression(string expression, Dictionary<string, Value> currentValues)
        {
			if(currentValues.Count == 0)
            {
				var finalValue = await CSharpScript.RunAsync(expression);
				return finalValue.ReturnValue.ToString();

			}
			int index = 0;
            var value = await CSharpScript.RunAsync($"{currentValues.ElementAt(0).Value.dataType} {currentValues.ElementAt(0).Key} = {currentValues.ElementAt(0).Value.value};");
			for(int i = 1; i < currentValues.Count; i++)
            {
				KeyValuePair<string, Value> keyValuePair = currentValues.ElementAt(i);
				value = await value.ContinueWithAsync($"{keyValuePair.Value.dataType} {keyValuePair.Key} = {keyValuePair.Value.value};");
			}

			value = await value.ContinueWithAsync(expression);

			return value.ReturnValue.ToString();
        }

		public static async Task<string> evaluateFunction(string functionString, string expression, Dictionary<string, Value> currentValues)
        {
			//Console.WriteLine("Function Code: " + functionString);
			var finalValue = await CSharpScript.RunAsync(functionString);
			if (currentValues.Count == 0)
			{
				finalValue = await finalValue.ContinueWithAsync(expression);
				return finalValue.ReturnValue.ToString();

			}
			int index = 0;
			finalValue = await finalValue.ContinueWithAsync($"{currentValues.ElementAt(0).Value.dataType} {currentValues.ElementAt(0).Key} = {currentValues.ElementAt(0).Value.value};");
			for (int i = 1; i < currentValues.Count; i++)
			{
				KeyValuePair<string, Value> keyValuePair = currentValues.ElementAt(i);
				finalValue = await finalValue.ContinueWithAsync($"{keyValuePair.Value.dataType} {keyValuePair.Key} = {keyValuePair.Value.value};");
			}

			finalValue = await finalValue.ContinueWithAsync(expression);
			
			return finalValue.ReturnValue.ToString();

		}
	}
}


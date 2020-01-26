using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReversePolishNotation
{
    public class RPN
    {


        private readonly Dictionary<char, int> _allOperations;

        public RPN()
        {
            _allOperations = new Dictionary<char, int>
                {
                    { '(', -1},
                    { ')', -1},
                    { '-', 0},
                    { '+', 0},
                    { '/', 1},
                    { '*', 1},
                    { '^', 2}
                };
        }

        public Queue<string> PolishExpression(string expression)
        {

            Queue<string> polishExpression =new Queue<string>();
            Stack<char> operationsStack =new Stack<char>();
            string number = "";

            for (int i = 0; i < expression.Length; i++)
            {
                char symbol = expression[i];

                if (_allOperations.ContainsKey(symbol)) //if symbol is operation
                {
                    if (number != "") //add number to polish expression
                    {
                        polishExpression.Enqueue(number );
                        number = "";
                    }

                    else if ((symbol != '(' && i==0) || (i != 0 && expression[i - 1] != ')' && symbol != '(')) //operation can't stay before number
                    {
                        throw new Exception("Wrong expression");
                    }

                    if (operationsStack.Count == 0 || _allOperations[operationsStack.Peek()] < _allOperations[symbol]) //add operation to the queue of operations
                    {
                        operationsStack.Push(symbol);
                    }

                    else if (symbol != ')') //push last operation into polish expression
                    {
                        polishExpression.Enqueue(operationsStack.Pop().ToString());
                        operationsStack.Push(symbol);
                    }

                    else//push only operations in brackets
                    {
                        while (operationsStack.Peek() != '(')
                        {
                            polishExpression.Enqueue(operationsStack.Pop().ToString());
                        }

                        operationsStack.Pop();
                    }
                }

                else //create number
                {
                    number += symbol;
                }

                if (i == expression.Length - 1)
                {
                    polishExpression.Enqueue(number);

                    while (operationsStack.Count!=0)
                    {
                        polishExpression.Enqueue(operationsStack.Pop().ToString());
                    }
                }
            }


            return polishExpression;
        }

        public double Calculate(Queue<string> expression)
        {
            List<double> numbers = new List<double>();

            foreach (string symbol in expression)
            {
                try
                {
                    numbers.Add(Convert.ToDouble(symbol));
                }
                catch (Exception e)
                {
                    double result =
                        CalculateResultOfAnOperation(symbol.ToCharArray()[0], numbers[numbers.Count - 2],
                            numbers[numbers.Count - 1]);
                    numbers.RemoveRange(numbers.Count - 2, 2);
                    numbers.Add(result);
                }
            }

            return numbers[0];
        }

        public double CalculateResultOfAnOperation(char operation, double firstNumber, double secondNumber)
        {
            double result = 0;

            switch (operation)
            {
                case '-':
                    result = firstNumber - secondNumber;
                    break;

                case '+':
                    result = firstNumber + secondNumber;
                    break;

                case '/':
                    result = firstNumber / secondNumber;
                    break;

                case '*':
                    result = firstNumber * secondNumber;
                    break;

                case '^':
                    result = Math.Pow(firstNumber, secondNumber);
                    break;
            }

            return result;
        }

    }
}

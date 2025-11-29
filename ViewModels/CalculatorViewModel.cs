using MvvmCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace MvvmCalculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorModel _model = new CalculatorModel();

        private string _display = "0";
        private double _firstNumber = 0;
        private string _operator = "";
        private bool _isNewInput = true;

        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }

        public ICommand NumberCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand ClearCommand { get; }

        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand(param => AddNumber(param.ToString()));
            OperatorCommand = new RelayCommand(param => SetOperator(param.ToString()));
            EqualCommand = new RelayCommand(param => Calculate());
            ClearCommand = new RelayCommand(param => Clear());
        }

        private void AddNumber(string num)
        {
            if (_isNewInput)
            {
                Display = num;
                _isNewInput = false;
            }
            else
            {
                Display += num;
            }
        }

        private void SetOperator(string op)
        {
            _firstNumber = double.Parse(Display);
            _operator = op;
            _isNewInput = true;
        }

        private void Calculate()
        {
            double second = double.Parse(Display);
            double result = _model.Calculate(_firstNumber, second, _operator);
            Display = result.ToString();
            _isNewInput = true;
        }

        private void Clear()
        {
            Display = "0";
            _firstNumber = 0;
            _operator = "";
            _isNewInput = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

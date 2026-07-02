using System;

namespace task07
{
    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; set; }

        [DisplayName("Тестовый метод")]
        public void TestMethod()
        {
            Console.WriteLine("Тестовый метод");
        }
        
        public void MethodWithoutAttribute()
        {
            Console.WriteLine("Тестовый метод без атрибута");
        }
    }
}
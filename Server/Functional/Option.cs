namespace Functional
{
    public class Option<T>
    {
        public static implicit operator Option<T>(T content) => new Some<T>(content);
        public static implicit operator Option<T>(None none) => new None<T>();
    }

    public class Some<T> : Option<T>
    {
        private T Content { get; }

        public Some(T content)
        {
            Content = content;
        }

        public static implicit operator T(Some<T> value) => value.Content;
    }

    public class None<T> : Option<T>
    {
    }

    public class None
    {
        private None() { }
        public static None Value { get; } = new None();
    }

}

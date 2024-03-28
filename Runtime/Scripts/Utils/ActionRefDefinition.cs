
public static class ActionValRefDefinition
{
    public delegate void ActionRef<T>(ref T item);
    public delegate void ActionValRef<T1, T2>(T1 arg1, ref T2 arg2);
    public delegate void ActionValValRef<T1, T2, T3>(T1 arg1, T2 arg2, ref T3 arg3);
    public delegate void ActionValValValRef<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, ref T4 arg4);
    public delegate void ActionValValValValRef<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, ref T5 arg5);
}
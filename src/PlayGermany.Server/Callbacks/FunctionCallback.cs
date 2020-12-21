using System;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;

namespace PlayGermany.Server.Callbacks
{
    public class FunctionCallback<T>
        : IBaseObjectCallback<T>
        where T : IBaseObject
        {
            private readonly Action<T> _callback;

            public FunctionCallback(Action<T> callback)
            {
                _callback = callback;
            }

            public void OnBaseObject(T baseObject)
            {
                _callback(baseObject);
            }
        }
}

using TMPro;
using UnityEngine;

public interface IActorMightDictionaryKey
{
    string Name { get; }
}
public interface IActorMightDictionaryKeyPlus: IActorMightDictionaryKey
{
    Sprite Icon { get; }
    Color Color { get; }
}

public interface IActorMightReserver : IActorMightDictionaryKey { }
public interface IActorMightPreviewer : IActorMightDictionaryKey { }
public interface IActorMightMultiplier : IActorMightDictionaryKey { }
public interface IActorMighAddition : IActorMightDictionaryKey { }
public interface IActorMightMissing : IActorMightDictionaryKeyPlus { }

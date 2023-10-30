using Flazzy.ABC;

namespace Tanji.Core.Habbo.Canvas.Flash;

public readonly record struct FlashMessageReference(ASMethod Method, ASMethod? Callback, int OrderInMethod, int ArgumentsUsed);
using Cysharp.Threading.Tasks;
using GonDraz.Events;

namespace GonDraz.Interfaces
{
    public interface IAsyncInitProgress
    {
        UniTask InitAsync();
    }
}


// O código abaixo tem como objetivo simular e propor uma solução
// para um dos problemas mais comuns em servidores TCP, onde vários
// clientes acessam e modificam as mesmas listas gerando exceções e
// problemas de concorrência.

using System.Collections.Concurrent;
using static System.Console;

// Execute o problema ou solução.
await Problema();
//await Solucao();

async Task Problema()
{
    var itens = new List<Item>
    {
        new Item(),
        new Item(),
        new Item(),
        new Item(),
        new Item(),
        new Item(),
        new Item(),
        new Item(),
        new Item()
    };

    async Task Interar()
    {
        foreach (var item in itens)
        {
            WriteLine(item);
            await Task.Delay(1000);
        }
    }

    async Task Remover(int index)
    {
        await Task.Delay(2000);
        var item = itens[index];

        // Aqui ativa uma exceção no metódo 'Interar()'
        // pois a coleção já esta sendo utilizada dentro dele.
        itens.Remove(item);

        WriteLine($"{item} foi removido com sucesso!");
    }

    try
    {
        var tasks = new List<Task>
        {
            Interar(),
            Remover(8)
        };

        await Task.WhenAll(tasks);
    }
    catch (Exception e)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine(e.Message);
    }
}

async Task Solucao()
{
    // Com Dictionary também funciona, veja mais sobre
    // os benefícios do ConcurrentDictionary em: https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/when-to-use-a-thread-safe-collection#concurrentdictionary-vs-dictionary
    var itens = new ConcurrentDictionary<int, Item>();

    itens.TryAdd(0, new Item());
    itens.TryAdd(1, new Item());
    itens.TryAdd(2, new Item());
    itens.TryAdd(3, new Item());
    itens.TryAdd(4, new Item());
    itens.TryAdd(5, new Item());
    itens.TryAdd(6, new Item());
    itens.TryAdd(7, new Item());
    itens.TryAdd(8, new Item());

    async Task Interar()
    {
        foreach (var item in itens)
        {
            WriteLine(item.Value.Id);
            await Task.Delay(1000);
        }
    }

    async Task Remover(int key)
    {
        await Task.Delay(2000);

        if (itens.TryRemove(key, out var item))
        {
            WriteLine($"{item} foi removido com sucesso!");
        }
    }

    try
    {
        var tasks = new List<Task>
        {
            Interar(),
            Remover(8)
        };

        await Task.WhenAll(tasks);

        ForegroundColor = ConsoleColor.Green;
        WriteLine("Lista interada e modificada com sucesso!");
    }
    catch (Exception e)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine(e.Message);
    }
}

public class Item
{
    public Item()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public override string ToString() => $"Item com id {Id}";
}

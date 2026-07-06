namespace api_produtos;

public static class ProdutosEndpoints
{
    private static List<Produto> produtos = new();
    private static int proximoId = 1;

    public static void MapProdutos(this WebApplication app)
    {
        app.MapGet("/produtos", () => produtos);

        app.MapGet("/produtos/{id}", (int id) =>
        {
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            return produto is null ? Results.NotFound("Produto não encontrado") : Results.Ok(produto);
        });

        app.MapPost("/produtos", (Produto produto) =>
        {
            produto.Id = proximoId++;
            produtos.Add(produto);
            return Results.Created($"/produtos/{produto.Id}", produto);
        });

        app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
        {
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null) return Results.NotFound("Produto não encontrado");

            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Preco = produtoAtualizado.Preco;
            produto.Estoque = produtoAtualizado.Estoque;

            return Results.Ok(produto);
        });

        app.MapDelete("/produtos/{id}", (int id) =>
        {
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null) return Results.NotFound("Produto não encontrado");

            produtos.Remove(produto);
            return Results.Ok($"Produto {id} deletado com sucesso");
        });
    }
}
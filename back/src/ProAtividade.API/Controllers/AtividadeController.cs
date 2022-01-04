using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAtividade.Domain.Entities;
using ProAtividade.Domain.Interfaces.Services;

namespace ProAtividade.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AtividadeController : ControllerBase
  {
    private readonly IAtividadeService _atividadeService;
    public AtividadeController(IAtividadeService atividadeService)
    {
      _atividadeService = atividadeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var atividades = await _atividadeService.PegarTodasAtividadesAsync();
        if (atividades == null) return NoContent();

        return Ok(atividades);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
          $"Erro ao tentar recuperar Atividades. Erro: {ex.Message}");
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        var atividade = await _atividadeService.PegarAtividadePorIdAsync(id);
        if (atividade == null) return NoContent();

        return Ok(atividade);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
          $"Erro ao tentar recuperar Atividade com id {id}. Erro: {ex.Message}");
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Atividade atividade)
    {
      try
      {
        var result = await _atividadeService.AdicionarAtividade(atividade);
        if (result == null) return NoContent();

        return Ok(result);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
          $"Erro ao tentar criar a Atividade. Erro: {ex.Message}");
      }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Atividade atividade)
    {
      try
      {
        if (atividade.Id != id)
          this.StatusCode(StatusCodes.Status409Conflict,
            "Você está tentando atualizar a atividade errada");

        var updated = await _atividadeService.AtualizarAtividade(atividade);
        if (updated == null) return NoContent();

        return Ok(updated);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
          $"Erro ao tentar atualizar a Atividade com id {id}. Erro: {ex.Message}");
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var atividade = await _atividadeService.PegarAtividadePorIdAsync(id);
        if (atividade == null)
          this.StatusCode(StatusCodes.Status409Conflict,
              "Você está tentando deletar uma atividade que não existe!");

        if (await _atividadeService.DeletarAtividade(atividade.Id))
        {
          return Ok(new { message = "Deletado" });
        }
        else
        {
          return BadRequest("Ocorreu um problema não específico ao tentar deletar a atividade");
        }
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
          $"Erro ao tentar deletar a Atividade com id {id}. Erro: {ex.Message}");
      }
    }
  }
}
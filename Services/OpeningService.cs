// OpeningService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpeningExplorer.Data;
using OpeningExplorer.DTOs;
using OpeningExplorer.Entities;

namespace OpeningExplorer.Services;
public class OpeningService : IOpeningService {
    private readonly DataContext _ctx;
    private readonly IMapper _mapper;
    public OpeningService(DataContext ctx, IMapper mapper) { _ctx = ctx; _mapper = mapper; }

    public async Task<int> Create(CreateOpeningDto dto) {
        var entity = _mapper.Map<Opening>(dto);
        _ctx.Openings.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity.Id;
    }
    public async Task Delete(int id) {
        var o = await _ctx.Openings.FindAsync(id) ?? throw new KeyNotFoundException("Ouverture introuvable");
        _ctx.Openings.Remove(o);
        await _ctx.SaveChangesAsync();
    }
    public async Task<OpeningDto[]> GetAll() {
        var list = await _ctx.Openings.ToListAsync();
        return _mapper.Map<OpeningDto[]>(list);
    }
    public async Task<OpeningDto> Get(int id) {
        var o = await _ctx.Openings.FindAsync(id) ?? throw new KeyNotFoundException("Ouverture introuvable");
        return _mapper.Map<OpeningDto>(o);
    }
    public async Task Update(int id, CreateOpeningDto dto) {
        var o = await _ctx.Openings.FindAsync(id) ?? throw new KeyNotFoundException("Ouverture introuvable");
        _mapper.Map(dto, o);
        await _ctx.SaveChangesAsync();
    }
}
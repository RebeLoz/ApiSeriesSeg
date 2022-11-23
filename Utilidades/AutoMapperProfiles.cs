using AutoMapper;
using ApiSeries.DTOs;
using ApiSeries.Entidades;

namespace ApiSeries.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<SerieDTO, Serie>();
            CreateMap<Serie, GetSerieDTO>();
            CreateMap<Serie, SerieDTOConCategorias>()
                .ForMember(serieDTO => serieDTO.Categorias, opciones => opciones.MapFrom(MapSerieDTOCategorias));
            CreateMap<CategoriaCreacionDTO, Categoria>()
                .ForMember(categoria => categoria.SerieCategoria, opciones => opciones.MapFrom(MapSerieCategoria));
            CreateMap<Categoria, CategoriaDTO>();
            CreateMap<Categoria, CategoriaDTOConSeries>()
                .ForMember(categoriaDTO => categoriaDTO.Series, opciones => opciones.MapFrom(MapCategoriaDTOSeries));
            CreateMap<CategoriaPatchDTO, Categoria>().ReverseMap();
            CreateMap<TipoCreacionDTO, Tipos>();
            CreateMap<Tipos, TipoDTO>();
        }

        private List<CategoriaDTO> MapSerieDTOCategorias(Serie serie, GetSerieDTO getSerieDTO)
        {
            var result = new List<CategoriaDTO>();
            if (serie.SerieCategoria == null) { return result; }
            foreach (var serieCategoria in serie.SerieCategoria)
            {
                result.Add(new CategoriaDTO()
                {
                    Id = serieCategoria.CategoriaId,
                    Name = serieCategoria.Categoria.Name
                });
            }
            return result;
        }

        private List<GetSerieDTO> MapCategoriaDTOSeries(Categoria categoria, CategoriaDTO categoriaDTO)
        {
            var result = new List<GetSerieDTO>();
            if (categoria.SerieCategoria == null)
            {
                return result;
            }
            foreach (var seriecategoria in categoria.SerieCategoria)
            {
                result.Add(new GetSerieDTO()
                {
                    Id = seriecategoria.SerieId,
                    Name = seriecategoria.Serie.Name
                });
            }
            return result;
        }

        private List<SerieCategoria> MapSerieCategoria(CategoriaCreacionDTO categoriaCreacionDTO, Categoria categoria)
        {
            var resultado = new List<SerieCategoria>();

            if (categoriaCreacionDTO.SeriesIds == null) { return resultado; }
            foreach (var serieId in categoriaCreacionDTO.SeriesIds)
            {
                resultado.Add(new SerieCategoria() { SerieId = serieId });
            }
            return resultado;
        }
    }
}
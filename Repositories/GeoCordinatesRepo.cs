using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using fleaApi.Graph_Dijikstra;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class GeoCordinatesRepo : IGeoCordinatesRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public GeoCordinatesRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<PointDto>> GetLocationPath(GetDistanceDto getDistanceDto)
        {
            var cordinatesList = new List<Point>();
            var currentPoint = new Point
            {
                Id = 0,
                Longitude = getDistanceDto.CurrentPosition.Longitude,
                Latitude = getDistanceDto.CurrentPosition.Latitude,
                MarketId = getDistanceDto.MarketId,
                ShopId = getDistanceDto.ShopId
            };

            cordinatesList.Add(currentPoint);

            var entryPointOfMarket = await _context.Points
                .Where(x => x.MarketId == getDistanceDto.MarketId && x.ShopId == null && x.Status == StatusEnum.EntryPoint)
                .SingleOrDefaultAsync();

            cordinatesList.Add(entryPointOfMarket);

            var wayPointOfMarket = await _context.Points
                .Where(x => x.MarketId == getDistanceDto.MarketId && x.ShopId == null && x.Status == StatusEnum.Way)
                .OrderBy(x => x.Id)
                .ToListAsync();

            cordinatesList.AddRange(wayPointOfMarket);

            var shopPointOfMarket = await _context.Points
                .SingleOrDefaultAsync(x => x.MarketId == getDistanceDto.MarketId && x.ShopId == getDistanceDto.ShopId && x.Status == StatusEnum.CenterPoint);

            cordinatesList.Add(shopPointOfMarket);

            var graph = new WeightedGraph<Point>(false, false);

            foreach (var point in cordinatesList)
            {
                graph.AddNode(point);
            }

            var from = graph.Nodes.Find(x => x.Index == 0);
            var to = graph.Nodes.Find(x => x.Index == 1);
            graph.AddEdge(from, to, 1);

            for (int i = 1; i < cordinatesList.Count - 1; i++)
            {
                var fromNode = graph.Nodes.Find(x => x.Index == i);
                if (!string.IsNullOrEmpty(fromNode.Value.Neighbors))
                {
                    if (fromNode.Value.Neighbors.Contains('-'))
                    {
                        foreach (var node in fromNode.Value.Neighbors.Split('-'))
                        {
                            var toNode = graph.Nodes.Find(x => x.Value.Id == Convert.ToInt32(node));
                            if (toNode is not null) graph.AddEdge(fromNode, toNode, 1);
                        }
                    }
                    else
                    {
                        var toNode = graph.Nodes.Find(x => x.Value.Id == Convert.ToInt32(fromNode.Value.Neighbors));
                        if (toNode is not null) graph.AddEdge(fromNode, toNode, 1);
                    }
                }
            }

            var source = from;
            var target = graph.Nodes.Find(x => x.Index == cordinatesList.Count -1);
            
            var path = graph.GetShortestPathDijkstra(source, target);

            var pointDtoList = new List<PointDto>();
            foreach (var pathPoint in path)
            {
                var pointDto = new PointDto
                {
                    Longitude = pathPoint.From.Value.Longitude,
                    Latitude = pathPoint.From.Value.Latitude,
                    PointStatus = pathPoint.From.Value.Status.ToString()
                };
                pointDtoList.Add(pointDto);
            }

            return pointDtoList;
        }

        public async Task<IEnumerable<PointDto>> GetMarketCordinate(int id)
        {
            return await _context.Points
                .Where(x => x.MarketId == id && x.ShopId == null)
                .OrderBy(x => x.Id)
                .ProjectTo<PointDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointDto>> GetShopCordinate(int id)
        {
            return await _context.Points
                .Where(x => x.ShopId == id)
                .OrderBy(x => x.Id)
                .ProjectTo<PointDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
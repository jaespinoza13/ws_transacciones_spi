﻿using Application.Common.ISO20022.Models;
using Domain;

namespace Application.Features.Catalogos.Queries.GetIfis;

public class ResGetIfis : ResComun
{
    public IReadOnlyList<Ifi> lst_ifis { get; set; } = new List<Ifi>();
}
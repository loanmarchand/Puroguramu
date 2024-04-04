﻿namespace Puroguramu.Domains.Repository;

public interface IExercisesRepository
{
    int GetExercisesCount();

    Exercise GetExercise(string exerciseId);
}

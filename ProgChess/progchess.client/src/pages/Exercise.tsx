import { useEffect, useState } from "react";
import { useCookie } from "../providers/CookieProvider";
import ExerciseContent from "./ExerciseContent";
import api from "../utils/api";
import { useNavigate, useParams } from "react-router";
import type { Exercise, Error } from "../utils/type";
import ErrorPage from "./error/ErrorPage";
import { handleApiError } from "../utils/apiErrorHandler";
import Score from "./Score";
import { BadgeProvider } from "../providers/ShowBadgeProvider";

export default function Exercice() {
  const [exercise, setExercise] = useState<Exercise>();
  const [error, setError] = useState<Error>();
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();
  const { id } = useParams();
  const { cookie } = useCookie();

  useEffect(() => {
    if (cookie) {
      getExercise();
    } else {
      navigate(`/login/${id}`);
    }
  }, [cookie]);

  const getExercise = async () => {
    try {
      const response = await api.get(`/api/exercise/active/${id}`);
      setExercise(response.data);
    } catch (error) {
      handleApiError(error, setError);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <>
      {isLoading ? (
        <div className="h-screen bg-zinc-900 justify-center items-center">
          <svg
            className="mr-3 size-5 animate-spin ..."
            viewBox="0 0 24 24"
          ></svg>
        </div>
      ) : !error ? (
        exercise?.studentExercises[0].isComplete ? (
          <Score exerciseId={id} onError={setError} />
        ) : (
          <BadgeProvider>
            <ExerciseContent exercise={exercise} />
          </BadgeProvider>
        )
      ) : (
        <ErrorPage status={error.status} message={error.message} />
      )}
    </>
  );
}

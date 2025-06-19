import { useEffect, useState } from "react";
import api from "../utils/api";
import { handleApiError } from "../utils/apiErrorHandler";

interface ScoreProps {
  exerciseId: string | undefined;
}

export default function Score({ exerciseId }: ScoreProps) {
  const [result, setResult] = useState([]);

  const getScore = async () => {
    try {
      const response = await api.get(`/api/score/${exerciseId}/student-result`);
      setResult(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getScore();
  }, []);

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-zinc-900 text-white p-6">
      <div className="bg-zinc-800 p-8 rounded-2xl shadow-lg w-full max-w-md text-center">
        <h1 className="text-3xl font-bold mb-4">
          🎯 Résultat du test #{exerciseId}
        </h1>
        <p className="text-xl mb-6">
          Score :{" "}
          <span className="font-bold">
            {result.scoreValue} / {result.scoreTests?.length}
          </span>
        </p>

        {result.scoreTests?.length > 0 && (
          <ul className="space-y-2 text-left">
            {result.scoreTests.map((test: any, index: number) => (
              <li
                key={index}
                className={`p-3 rounded-lg flex items-center justify-between ${
                  test.isSuccess ? "bg-green-600" : "bg-red-600"
                }`}
              >
                <span>{test.name}</span>
                <span>{test.isSuccess ? "✅ Réussi" : "❌ Échoué"}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}

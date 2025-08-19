import { useEffect, useState, type SetStateAction } from "react";
import api from "../utils/api";
import { handleApiError } from "../utils/apiErrorHandler";
import CodeEditor from "../components/form/input/CodeEditor";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "../components/ui/accordion";

interface ScoreProps {
  exerciseId: string | undefined;
  onError: SetStateAction<any>;
}

export default function Score({ exerciseId, onError }: ScoreProps) {
  const [result, setResult] = useState();

  const getScore = async () => {
    try {
      const response = await api.get(`/api/score/${exerciseId}/student-result`);
      console.log(response.data);
      setResult(response.data);
    } catch (error) {
      handleApiError(error);
      onError("Erreur lors de la recherche du résultat");
    }
  };

  useEffect(() => {
    getScore();
  }, []);

  return (
    <div className="min-h-screen bg-zinc-900 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-5xl mx-auto space-y-10">
        <div className="text-center space-y-2">
          <h2 className="text-2xl md:text-4xl font-extrabold text-white">
            🎯 Résultat du test #{exerciseId}
          </h2>
          <p className="text-zinc-300 text-md md:text-lg">
            Voici votre code et vos résultats aux tests
          </p>
        </div>

        {result?.scoreTests?.every((test) => test.isSuccess === true) && (
          <div className="flex justify-center">
            <span className="inline-flex items-center gap-2 px-5 py-2 bg-gradient-to-r from-green-400 to-green-600 text-white text-sm font-semibold rounded-full shadow-lg animate-pulse">
              🎉 Félicitations ! Résultat parfait
            </span>
          </div>
        )}

        <div className="rounded-xl overflow-hidden border border-zinc-700 shadow-inner">
          <CodeEditor
            value={result?.answer ?? ""}
            height={window.innerHeight / 2}
            onChange={() => {}}
            editable={false}
          />
        </div>

        <div className="bg-zinc-800 shadow-lg rounded-2xl p-6 space-y-6 text-white">
          <h2 className="text-lg md:text-xl font-bold border-b border-zinc-700 pb-2">
            ✅ Résultats des tests unitaires
          </h2>

          {result?.scoreTests?.length ? (
            <Accordion className="space-y-3" type="single" collapsible>
              {result?.scoreTests.map((test, index) => (
                <AccordionItem
                  key={index}
                  value={`item-${index}`}
                  className="overflow-hidden border border-zinc-700 rounded-xl"
                >
                  <AccordionTrigger
                    className={`flex items-center justify-between px-4 py-4 text-sm sm:text-base font-medium transition-all cursor-pointer ${
                      test.isSuccess
                        ? "bg-green-600 hover:bg-green-700"
                        : "bg-red-600 hover:bg-red-700"
                    } text-white rounded-t-xl`}
                  >
                    <span className="truncate max-w-[80%] md:max-w-none">
                      {test.name}
                    </span>
                  </AccordionTrigger>
                  <AccordionContent
                    className={`transition-all duration-300 ease-in-out px-4 py-4 text-sm sm:text-base leading-relaxed ${
                      test.isSuccess
                        ? "bg-green-50 text-green-700"
                        : "bg-red-50 text-red-700"
                    } rounded-b-xl`}
                  >
                    <p>
                      {test.actual
                        ? `🔎 Résultat actuel : ${test.actual}`
                        : "Aucune information disponible"}
                    </p>
                    {test.expected && (
                      <p>🎯 Résultat attendu : {test.expected}</p>
                    )}
                  </AccordionContent>
                </AccordionItem>
              ))}
            </Accordion>
          ) : (
            <div className="text-center text-zinc-400">
              Aucun résultat de test disponible
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

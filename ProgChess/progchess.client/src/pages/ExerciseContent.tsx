import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router";
import api from "../utils/api";
import CookieProvider, { useCookie } from "../providers/CookieProvider";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExerciseCard from "../components/card/ExerciseCard";
import { Book, Braces, CheckCheck, MonitorDown } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercise } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";

export default function ExerciseContent() {
  const [exercise, setExercise] = useState<Exercise>();
  const [code, setCode] = useState<string>("");
  const codeRef = useRef("");
  const navigate = useNavigate();
  const { cookie, isLoading } = useCookie() || {};
  const { id } = useParams();

  useEffect(() => {
    if (cookie) {
      getExercise();
    } else {
      navigate("/login");
    }
  }, []);

  useEffect(() => {
    codeRef.current = code;
  }, [code]);

  const getExercise = async () => {
    try {
      const response = await api.get(`/api/exercise/${id}`);
      setExercise(response.data);
      const startedCode = localStorage.getItem("code");
      if (startedCode) {
        setCode(startedCode);
      } else {
        setCode(response.data.baseCode);
      }
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    const interval = setInterval(() => saveCode(), 30000);
    return () => clearInterval(interval);
  }, []);

  const saveCode = () => {
    localStorage.setItem("code", codeRef.current);
  };

  return (
    <CookieProvider>
      {isLoading ? (
        <div>
          <svg
            className="mr-3 size-5 animate-spin ..."
            viewBox="0 0 24 24"
          ></svg>
          Processing…
        </div>
      ) : (
        <div className="min-h-screen bg-zinc-900">
          <div className="flex py-2 px-4 items-center justify-between">
            <h2 className="text-2xl  text-green-500 font-semibold">
              ProgChess
            </h2>
            <Button
              className="bg-zinc-500 hover:bg-zinc-600 cursor-pointer"
              type="button"
              onClick={() => {
                saveCode();
                toast.success("Exercice mis à jour");
              }}
            >
              <MonitorDown />
              Sauvegarder
            </Button>
          </div>
          <div
            className="hidden w-full text-white px-4 py-2 sm:grid grid-rows-[auto_50%]
 grid-cols-[min-content_auto]"
          >
            <HorizontalResizable>
              <ExerciseCard title="Situation" icon={Book} canExecute={false}>
                <div className="overflow-auto p-4">
                  <MarkdownComponent markdown={exercise?.situation!} />
                </div>
              </ExerciseCard>
            </HorizontalResizable>
            <ExerciseCard title="Code" icon={Braces} canExecute={true}>
              <CodeEditor value={code} onChange={(e) => setCode(e)} />
            </ExerciseCard>
            <VerticalResizable>
              <TestCaseCard
                unitTests={exercise?.unitTests!.filter((ut) => ut.isActive)!}
              >
                <p></p>
              </TestCaseCard>
            </VerticalResizable>
          </div>
          <div className="grid md:hidden h-full gap-2 flex-1 px-4 space-y-4 bg-zinc-900 text-white">
            <ExerciseCard title="Situation" icon={Book} canExecute={false}>
              <div className="overflow-auto p-4">
                <MarkdownComponent markdown={exercise?.situation!} />
              </div>
            </ExerciseCard>

            <ExerciseCard title="Code" icon={Braces} canExecute={true}>
              <CodeEditor value={code} onChange={(e) => setCode(e)} />
            </ExerciseCard>

            <ExerciseCard
              title="Résultats"
              icon={CheckCheck}
              canExecute={false}
            >
              <p>Voici les résultats</p>
            </ExerciseCard>
          </div>
        </div>
      )}
    </CookieProvider>
  );
}

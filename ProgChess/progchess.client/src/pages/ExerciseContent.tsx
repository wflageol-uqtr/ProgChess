import { useEffect, useRef, useState, useTransition } from "react";
import { useNavigate, useParams } from "react-router";
import api from "../utils/api";
import CookieProvider, { useCookie } from "../providers/CookieProvider";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExerciseCard from "../components/card/ExerciseCard";
import {
  Book,
  Braces,
  CheckCheck,
  CheckLine,
  MonitorDown,
  X,
} from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercise } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";
import axios from "axios";

export default function ExerciseContent() {
  const [isPending, startTransition] = useTransition();
  const [exercise, setExercise] = useState<Exercise>();
  const [code, setCode] = useState<string>("");
  const codeRef = useRef("");
  const navigate = useNavigate();
  const { cookie, isLoading } = useCookie() || {};
  const { id } = useParams();
  const [height, setHeight] = useState(
    parseInt(localStorage.getItem("topHeight")!) || window.innerHeight / 2
  );

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

  useEffect(() => {
    localStorage.setItem("topHeight", height.toString());
  }, [height]);

  const getExercise = async () => {
    try {
      const response = await api.get(`/api/exercise/${id}`);
      setExercise(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    const startedCode = localStorage.getItem(`code:${exercise?.id}`);
    if (startedCode) {
      setCode(startedCode);
    } else {
      setCode(exercise?.baseCode!);
    }

    const interval = setInterval(() => saveCode(), 30000);
    return () => clearInterval(interval);
  }, [exercise]);

  const saveCode = () => {
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
  };

  const executeCode = async () => {
    startTransition(async () => {
      console.log("oco");

      try {
        const response = await axios.post(
          "http://localhost:5290/api/execute",
          {
            exerciseId: exercise?.id,
            code,
          },
          { withCredentials: true }
        );
        console.log(response);
      } catch (error) {}
    });
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
          <div className="hidden h-screen sm:grid grid-rows-1 text-white">
            <div className="grid grid-cols-[min-content_auto]">
              <HorizontalResizable>
                <ExerciseCard title="Situation" icon={Book} canExecute={false}>
                  <div className="overflow-auto p-4">
                    <MarkdownComponent markdown={exercise?.situation!} />
                  </div>
                </ExerciseCard>
              </HorizontalResizable>
              <div className="h-full grid grid-rows-[min-content_auto]">
                <VerticalResizable height={height} setHeight={setHeight}>
                  <ExerciseCard
                    isPending={isPending}
                    title="Code"
                    icon={Braces}
                    canExecute={true}
                    actionFn={executeCode}
                  >
                    <CodeEditor
                      height={height}
                      value={code}
                      onChange={(e) => setCode(e)}
                    />
                  </ExerciseCard>
                </VerticalResizable>

                <TestCaseCard
                  unitTests={exercise?.unitTests!.filter((ut) => ut.isActive)!}
                >
                  <div className="flex justify-end mt-1">
                    <div className="flex items-center gap-3 px-3 py-2 bg-zinc-700 rounded-lg text-sm">
                      <span className="text-zinc-300">(4 tests)</span>

                      <div className="flex items-center gap-1">
                        <CheckLine className="w-4 h-4 text-green-500" />
                        <span className="text-green-400">2</span>
                      </div>

                      <div className="flex items-center gap-1">
                        <X className="w-4 h-4 text-red-500" />
                        <span className="text-red-400">2</span>
                      </div>
                    </div>
                  </div>
                </TestCaseCard>
              </div>
            </div>
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

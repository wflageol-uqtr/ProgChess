import { useEffect, useRef, useState, useTransition } from "react";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExerciseCard from "../components/card/ExerciseCard";
import { Book, Braces, MonitorDown, RefreshCcw } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercise, TestResult } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";
import axios from "axios";
import { DeleteDialog } from "../components/dialog/DeleteDialog";

interface ExerciseContentProps {
  exercise?: Exercise;
}

export default function ExerciseContent({ exercise }: ExerciseContentProps) {
  const [openDialog, setOpenDialog] = useState(false);
  const [isPending, startTransition] = useTransition();
  const [testResult, setTestResult] = useState<TestResult[]>([]);
  const [code, setCode] = useState<string>("");
  const [testCode, setTestCode] = useState<string>("");
  const codeRef = useRef("");
  const [height, setHeight] = useState(
    parseInt(localStorage.getItem("topHeight")!) || window.innerHeight / 2
  );

  useEffect(() => {
    codeRef.current = code;
  }, [code]);

  useEffect(() => {
    localStorage.setItem("topHeight", height.toString());
  }, [height]);

  useEffect(() => {
    const startedCode = localStorage.getItem(`code:${exercise?.id}`);
    if (startedCode) {
      setCode(startedCode);
    } else {
      setCode(exercise?.baseCode!);
    }

    setTestCode(exercise?.unitTests[0].code! || "");

    const interval = setInterval(() => saveCode(), 30000);
    return () => clearInterval(interval);
  }, [exercise]);

  const saveCode = () => {
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
  };

  const executeCode = async () => {
    startTransition(async () => {
      try {
        const response = await axios.post(
          "http://localhost:5290/api/execute",
          {
            exerciseId: exercise?.id,
            code,
            unitTest: testCode,
          },
          { withCredentials: true }
        );
        saveCode();
        setTestResult(response.data);
        toast.success("Test exécuté");
      } catch (error) {
        toast.error("Une erreur est survenue lors de l'exécution");
      }
    });
  };

  const deleteCode = () => {
    codeRef.current = exercise?.baseCode || "";
    setCode(exercise?.baseCode || "");
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
    setOpenDialog(false);
    toast.success("Exercice réinitialiser");
  };

  return (
    <>
      <div className="min-h-screen bg-zinc-900">
        <div className="flex py-2 px-4 items-center justify-between">
          <h2 className="text-2xl  text-green-500 font-semibold">ProgChess</h2>
          <div className="space-x-2">
            <Button
              className="bg-zinc-500 hover:bg-zinc-600 cursor-pointer"
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setOpenDialog(true);
              }}
            >
              <RefreshCcw />
              Réinitialiser
            </Button>
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
        </div>
        <div className="hidden h-screen sm:grid grid-rows-1 text-white">
          <div className="grid grid-cols-[min-content_auto]">
            <HorizontalResizable>
              <ExerciseCard title="Situation" icon={Book} canExecute={false}>
                <div className="p-4">
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
                isPending={isPending}
                testResult={testResult}
                unitTestCode={testCode}
                setTestCode={setTestCode}
              />
            </div>
          </div>
        </div>
      </div>
      <DeleteDialog
        open={openDialog}
        message="Cette action est irréversible. Le code que vous avez jusqu'à présent sera perdu."
        onOpenChange={setOpenDialog}
        deleteFn={deleteCode}
      />
    </>
  );
}

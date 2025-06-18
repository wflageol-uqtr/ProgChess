import { useEffect, useRef, useState, useTransition } from "react";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExerciseCard from "../components/card/ExerciseCard";
import { Book, Braces, Check, MonitorDown, RefreshCcw } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercise, TestResult } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";
import axios from "axios";
import { DeleteDialog } from "../components/dialog/DeleteDialog";
import { useNavigate } from "react-router";
import SubmitDialog from "../components/dialog/SubmitDialog";
import { handleApiError } from "../utils/apiErrorHandler";

interface ExerciseContentProps {
  exercise?: Exercise;
}

export default function ExerciseContent({ exercise }: ExerciseContentProps) {
  const navigate = useNavigate();
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [openSubmitDialog, setOpenSubmitDialog] = useState(false);
  const [isPending, startTransition] = useTransition();
  const [testResult, setTestResult] = useState<TestResult[]>([]);
  const [code, setCode] = useState<string>("");
  const [testCode, setTestCode] = useState<string>("");
  const codeRef = useRef("");
  const unitTestRef = useRef("");
  const [height, setHeight] = useState(
    parseInt(localStorage.getItem("topHeight")!) || window.innerHeight / 2
  );
  const [disabledSelect, setDisabledSelect] = useState(false);

  useEffect(() => {
    codeRef.current = code;
  }, [code]);

  useEffect(() => {
    unitTestRef.current = testCode;
  }, [testCode]);

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

    const startedUnitTest = localStorage.getItem(`unitTest:${exercise?.id}`);
    if (startedUnitTest) {
      setTestCode(startedUnitTest);
    } else {
      setTestCode(exercise?.unitTests?.[0]?.code || "");
    }

    const interval = setInterval(() => saveCode(), 30000);
    return () => clearInterval(interval);
  }, [exercise]);

  const saveCode = () => {
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
    localStorage.setItem(`unitTest:${exercise?.id}`, unitTestRef.current);
  };

  const executeCode = async () => {
    startTransition(async () => {
      try {
        const response = await axios.post(
          "http://localhost:5290/api/execute",
          {
            code,
            unitTest: testCode,
          },
          { withCredentials: true }
        );
        saveCode();
        setTestResult(response.data.value);
        toast.success("Test exécuté");
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const submitCode = async () => {
    startTransition(async () => {
      try {
        const response = await axios.post(
          "http://localhost:5290/api/execute/submit",
          {
            exerciseId: exercise?.id,
            code,
          },
          { withCredentials: true }
        );
        navigate("/score", {
          state: {
            score: response.data.score,
            testResult: response.data.results,
          },
        });
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const deleteCode = () => {
    codeRef.current = exercise?.baseCode || "";
    setCode(exercise?.baseCode || "");
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
    unitTestRef.current = exercise?.unitTests?.[0]?.code || "";
    setTestCode(exercise?.unitTests?.[0]?.code || "");
    localStorage.setItem(`unitTest:${exercise?.id}`, codeRef.current);
    setOpenDeleteDialog(false);
    toast.success("Exercice réinitialiser");
  };

  return (
    <>
      <div className="min-h-screen bg-zinc-900 overflow-auto">
        <div className="flex py-2 px-4 items-center justify-between">
          <h2 className="text-2xl  text-green-500 font-semibold">ProgChess</h2>
          <div className="space-x-2">
            <Button
              className="bg-zinc-500 hover:bg-zinc-600 cursor-pointer"
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setOpenDeleteDialog(true);
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
            <Button
              className="bg-green-500 hover:bg-green-600 cursor-pointer"
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setOpenSubmitDialog(true);
              }}
            >
              <Check />
              Soummettre
            </Button>
          </div>
        </div>
        <div
          className={`hidden h-screen sm:grid grid-rows-1 text-white ${
            disabledSelect ? "select-none" : ""
          }`}
        >
          <div className="grid grid-cols-[min-content_auto]">
            <HorizontalResizable setDisabledSelect={setDisabledSelect}>
              <ExerciseCard title="Situation" icon={Book} canExecute={false}>
                <div className="p-4">
                  <MarkdownComponent markdown={exercise?.situation!} />
                </div>
              </ExerciseCard>
            </HorizontalResizable>
            <div className="h-full grid grid-rows-[min-content_auto]">
              <VerticalResizable
                setDisabledSelect={setDisabledSelect}
                height={height}
                setHeight={setHeight}
              >
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
        open={openDeleteDialog}
        message="Cette action est irréversible. Le code que vous avez jusqu'à présent sera perdu."
        onOpenChange={setOpenDeleteDialog}
        deleteFn={deleteCode}
      />
      <SubmitDialog
        open={openSubmitDialog}
        onOpenChange={setOpenSubmitDialog}
        submitFn={() => submitCode()}
      />
    </>
  );
}

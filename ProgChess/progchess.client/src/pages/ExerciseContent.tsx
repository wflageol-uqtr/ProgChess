import { useEffect, useRef, useState, useTransition } from "react";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExecutableCard from "../components/card/ExecutableCard";
import { Book, Braces, Check, MonitorDown } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercise, TestResult } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";
import axios from "axios";
import { useNavigate } from "react-router";
import SubmitDialog from "../components/dialog/SubmitDialog";
import { handleApiError } from "../utils/apiErrorHandler";
import { useBadge } from "../providers/ShowBadgeProvider";
import SituationCard from "../components/card/SituationCard";

interface ExerciseContentProps {
  exercise?: Exercise;
}

export default function ExerciseContent({ exercise }: ExerciseContentProps) {
  const navigate = useNavigate();
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
  const [executionError, setExecutionError] = useState("");
  const { setBadgeTabs } = useBadge();

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
        setBadgeTabs((prev) => ({
          ...prev,
          result: true,
        }));
        toast.success("Test exécuté");
      } catch (error) {
        handleApiError(error, setExecutionError);
        setBadgeTabs((prev) => ({
          ...prev,
          errors: true,
        }));
      }
    });
  };

  const submitCode = async () => {
    startTransition(async () => {
      try {
        await axios.post(
          "http://localhost:5290/api/execute/submit",
          {
            exerciseId: exercise?.id,
            code,
          },
          { withCredentials: true }
        );
        navigate(0);
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const deleteCode = () => {
    codeRef.current = exercise?.baseCode || "";
    setCode(exercise?.baseCode || "");
    localStorage.setItem(`code:${exercise?.id}`, codeRef.current);
    toast.success("Exercice réinitialiser");
  };

  const deleteUnitTest = () => {
    unitTestRef.current = exercise?.unitTests?.[0]?.code || "";
    setTestCode(exercise?.unitTests?.[0]?.code || "");
    localStorage.setItem(`unitTest:${exercise?.id}`, unitTestRef.current);
    toast.success("Test réinitialiser");
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
              <SituationCard title="Situation" icon={Book}>
                <div className="p-4">
                  <MarkdownComponent markdown={exercise?.situation!} />
                </div>
              </SituationCard>
            </HorizontalResizable>
            <div className="h-full grid grid-rows-[min-content_auto]">
              <VerticalResizable
                setDisabledSelect={setDisabledSelect}
                height={height}
                setHeight={setHeight}
              >
                <ExecutableCard
                  isPending={isPending}
                  title="Code"
                  icon={Braces}
                  actionFn={executeCode}
                  reinitializeFn={deleteCode}
                >
                  <CodeEditor
                    height={height}
                    value={code}
                    onChange={(e) => setCode(e)}
                  />
                </ExecutableCard>
              </VerticalResizable>

              <TestCaseCard
                isPending={isPending}
                testResult={testResult}
                unitTestCode={testCode}
                setTestCode={setTestCode}
                reinitializeFn={deleteUnitTest}
                executionError={executionError}
              />
            </div>
          </div>
        </div>
      </div>
      <SubmitDialog
        open={openSubmitDialog}
        onOpenChange={setOpenSubmitDialog}
        submitFn={() => submitCode()}
      />
    </>
  );
}

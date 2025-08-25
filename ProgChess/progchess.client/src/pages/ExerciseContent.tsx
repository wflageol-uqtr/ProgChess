import { useEffect, useRef, useState, useTransition } from "react";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExecutableCard from "../components/card/ExecutableCard";
import { Book, Braces, Check, MonitorDown } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type {
  EditorError,
  EditorInfo,
  Exercise,
  TestResult,
} from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";
import TestCaseCard from "../components/card/TestCaseCard";
import { Button } from "../components/ui/button";
import { toast } from "sonner";
import axios from "axios";
import { useNavigate } from "react-router";
import SubmitDialog from "../components/dialog/SubmitDialog";
import { handleApiError, handleExecutionError } from "../utils/apiErrorHandler";
import { useBadge } from "../providers/ShowBadgeProvider";
import SituationCard from "../components/card/SituationCard";
import { apiUrl } from "../utils/api";
import ProgChessLoader from "../components/loader/ProgChessLoader";

interface ExerciseContentProps {
  exercise?: Exercise;
}

export default function ExerciseContent({ exercise }: ExerciseContentProps) {
  const navigate = useNavigate();
  const [openSubmitDialog, setOpenSubmitDialog] = useState(false);
  const [isPending, startTransition] = useTransition();
  const [isExecuting, startExecution] = useTransition();
  const [testResult, setTestResult] = useState<TestResult[]>([]);
  const [code, setCode] = useState<string>("");
  const [testCode, setTestCode] = useState<string>("");
  const codeRef = useRef("");
  const unitTestRef = useRef("");
  const [height, setHeight] = useState(
    parseInt(localStorage.getItem("topHeight")!) || window.innerHeight / 2
  );
  const [situationHeight, setSituationHeight] = useState(
    parseInt(localStorage.getItem("situationHeight")!) || window.innerHeight / 2
  );
  const [disabledSelect, setDisabledSelect] = useState(false);
  const [executionError, setExecutionError] = useState("");
  const [errorEditor, setErrorEditor] = useState<EditorError>();
  const editorInfo: EditorInfo[] = [];
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
    localStorage.setItem("situationHeight", situationHeight.toString());
  }, [situationHeight]);

  useEffect(() => {
    const startedCode = localStorage.getItem(
      `code:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`
    );
    if (startedCode) {
      setCode(startedCode);
    } else {
      setCode(exercise?.baseCode!);
    }

    const startedUnitTest = localStorage.getItem(
      `unitTest:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`
    );
    if (startedUnitTest) {
      setTestCode(startedUnitTest);
    } else {
      setTestCode(exercise?.unitTests?.[0]?.code || "");
    }

    const interval = setInterval(() => saveCode(), 30000);
    return () => clearInterval(interval);
  }, [exercise]);

  const saveCode = () => {
    localStorage.setItem(
      `code:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`,
      codeRef.current
    );
    localStorage.setItem(
      `unitTest:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`,
      unitTestRef.current
    );
  };

  const executeCode = async () => {
    startExecution(async () => {
      try {
        setExecutionError("");
        setErrorEditor(undefined);

        editorInfo.push(
          {
            name: "code",
            size: countLines(code),
          },
          {
            name: "test0",
            size: countLines(testCode),
          }
        );
        const response = await axios.post(
          `${apiUrl}/api/execute`,
          {
            code,
            unitTest: testCode,
          },
          { withCredentials: true }
        );
        saveCode();
        setTestResult(response.data.value);
        setBadgeTabs((prev: any) => ({
          ...prev,
          0: true,
        }));
        toast.success("Test(s) exécuté(s) avec succès");
      } catch (error: any) {
        if (error?.status === 600) {
          const errorObj = handleExecutionError(
            editorInfo,
            error.response?.data?.detail
          );
          setErrorEditor(errorObj);
          handleApiError(error, setExecutionError);
          setBadgeTabs((prev: any) => ({
            ...prev,
            2: true,
          }));
          return;
        }
        handleApiError(error);
      }
    });
  };

  const submitCode = async () => {
    startTransition(async () => {
      setExecutionError("");
      setErrorEditor(undefined);
      try {
        await axios.post(
          `${apiUrl}/api/execute/submit`,
          {
            exerciseId: exercise?.id,
            code,
          },
          { withCredentials: true }
        );
        navigate(0);
      } catch (error: any) {
        handleApiError(error);
        if (error?.status === 600) {
          const errorObj = handleExecutionError(
            editorInfo,
            error.response?.data?.detail
          );
          setErrorEditor(errorObj);
          handleApiError(error, setExecutionError);
          setBadgeTabs((prev: any) => ({
            ...prev,
            2: true,
          }));
          setOpenSubmitDialog(false);
          return;
        }
        handleApiError(error, setExecutionError);
      }
    });
  };

  const deleteCode = () => {
    codeRef.current = exercise?.baseCode || "";
    setCode(exercise?.baseCode || "");
    localStorage.setItem(
      `code:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`,
      codeRef.current
    );
    toast.success("Exercice réinitialiser");
  };

  const deleteUnitTest = () => {
    unitTestRef.current = exercise?.unitTests?.[0]?.code || "";
    setTestCode(exercise?.unitTests?.[0]?.code || "");
    localStorage.setItem(
      `unitTest:${exercise?.id}${exercise?.studentExercises[0].studentPermanentCode}`,
      unitTestRef.current
    );
    toast.success("Test réinitialiser");
  };

  const countLines = (str: string) => {
    if (str.trim() === "") return 0;
    return str.split("\n").length;
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
              <div className="hidden md:flex">Sauvegarder</div>
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
              <div className="hidden md:flex">Soummettre</div>
            </Button>
          </div>
        </div>
        <div
          className={`h-screen sm:grid grid-rows-1 text-white ${
            disabledSelect ? "select-none" : ""
          }`}
        >
          <div className="grid grid-cols-1 md:grid-cols-[min-content_auto]">
            <div className="flex md:hidden">
              <VerticalResizable
                height={situationHeight}
                setHeight={setSituationHeight}
                setDisabledSelect={setDisabledSelect}
              >
                <SituationCard title="Situation" icon={Book}>
                  <div className="p-4">
                    <MarkdownComponent markdown={exercise?.situation!} />
                  </div>
                </SituationCard>
              </VerticalResizable>
            </div>
            <div className="hidden md:block">
              <HorizontalResizable setDisabledSelect={setDisabledSelect}>
                <SituationCard title="Situation" icon={Book}>
                  <div className="p-4">
                    <MarkdownComponent markdown={exercise?.situation!} />
                  </div>
                </SituationCard>
              </HorizontalResizable>
            </div>
            <div className="h-full grid grid-rows-[min-content_auto]">
              <VerticalResizable
                setDisabledSelect={setDisabledSelect}
                height={height}
                setHeight={setHeight}
              >
                <ExecutableCard
                  isPending={isExecuting}
                  title="Code"
                  icon={Braces}
                  actionFn={executeCode}
                  reinitializeFn={deleteCode}
                >
                  <div className="relative">
                    <CodeEditor
                      height={height}
                      value={code}
                      onChange={(e) => setCode(e)}
                      executionError={
                        errorEditor?.id === "code" ? errorEditor : undefined
                      }
                    />
                    {isExecuting && <ProgChessLoader />}
                  </div>
                </ExecutableCard>
              </VerticalResizable>

              <TestCaseCard
                isPending={isPending}
                testResult={testResult}
                unitTestCode={testCode}
                setTestCode={setTestCode}
                reinitializeFn={deleteUnitTest}
                executionError={errorEditor ?? undefined}
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

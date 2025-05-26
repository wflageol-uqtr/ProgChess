import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import api from "../utils/api";
import CookieProvider, { useCookie } from "../providers/CookieProvider";
import HorizontalResizable from "../components/layout/HorizontalResizable";
import VerticalResizable from "../components/layout/VerticalResizable";
import ExerciceCard from "../components/card/ExerciceCard";
import { Book, Braces, CheckCheck } from "lucide-react";
import MarkdownComponent from "../components/form/input/MarkdownComponent";
import type { Exercice } from "../utils/type";
import CodeEditor from "../components/form/input/CodeEditor";

export default function ExerciceContent() {
  const [exerice, setExercice] = useState<Exercice>();
  const [code, setCode] = useState<string>("");
  const navigate = useNavigate();
  const { cookie, isLoading } = useCookie() || {};
  const { id } = useParams();

  useEffect(() => {
    if (cookie) {
      getExercice();
    } else {
      navigate("/login");
    }
  }, [cookie]);

  const getExercice = async () => {
    try {
      const response = await api.get(`/api/exercice/${id}`);
      setExercice(response.data);
    } catch (error) {
      console.log(error);
    }
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
          <h2 className="text-2xl px-4 text-green-500 font-semibold">
            ProgChess
          </h2>
          <div className="hidden w-full text-white px-4 py-2 sm:grid grid-rows-[50%_auto] grid-cols-[min-content_auto] max-h-9/10">
            <HorizontalResizable>
              <ExerciceCard title="Situation" icon={Book} canExecute={false}>
                <div className="overflow-auto p-4">
                  <MarkdownComponent markdown={exerice?.situation!} />
                </div>
              </ExerciceCard>
            </HorizontalResizable>
            <ExerciceCard title="Code" icon={Braces} canExecute={true}>
              <CodeEditor value={exerice?.baseCode!} onChange={setCode} />
            </ExerciceCard>
            <VerticalResizable>
              <ExerciceCard
                title="Résultats"
                icon={CheckCheck}
                canExecute={false}
              >
                <p> Resulat</p>
              </ExerciceCard>
            </VerticalResizable>
          </div>
          <div className="grid md:hidden h-full gap-2 flex-1 px-4 space-y-4 bg-zinc-900 text-white">
            <ExerciceCard title="Situation" icon={Book} canExecute={false}>
              <div className="overflow-auto p-4">
                <MarkdownComponent markdown={exerice?.situation!} />
              </div>
            </ExerciceCard>

            <ExerciceCard title="Code" icon={Braces} canExecute={true}>
              <CodeEditor value={exerice?.baseCode!} onChange={setCode} />
            </ExerciceCard>

            <ExerciceCard
              title="Résultats"
              icon={CheckCheck}
              canExecute={false}
            >
              <p>Voici les résultats</p>
            </ExerciceCard>
          </div>
        </div>
      )}
    </CookieProvider>
  );
}

:- dynamic availability/3.
:- dynamic agenda_staff/3.
:- dynamic agenda_staff1/3.
:- dynamic agenda_operation_room/3.
:- dynamic agenda_operation_room1/3.
:- dynamic heuristic_sol/5.


agenda_staff(d001,20241028,[(720,790,m01),(1080,1140,c01)]).
agenda_staff(d002,20241028,[(850,900,m02),(901,960,m02),(1380,1440,c02)]).
agenda_staff(d003,20241028,[(720,790,m01),(910,980,m02)]).
%agenda_staff(d004,20241028,[(850,900,m02),(940,980,c04)]).

timetable(d001,20241028,(480,1200)).
timetable(d002,20241028,(500,1440)).
timetable(d003,20241028,(520,1320)).
%timetable(d004,20241028,(620,1020)).


staff(d001,doctor,orthopaedist,[so2,so3,so4]).
staff(d002,doctor,orthopaedist,[so2,so3,so4]).
staff(d003,doctor,orthopaedist,[so2,so3,so4]).

%surgery(SurgeryType,TAnesthesia,TSurgery,TCleaning).

surgery(so2,45,60,45).
surgery(so3,45,90,45).
surgery(so4,45,75,45).

surgery_id(so100001,so2).
surgery_id(so100002,so3).
surgery_id(so100003,so4).
%surgery_id(so100004,so2).
%surgery_id(so100005,so4).
%surgery_id(so100006,so2).
%surgery_id(so100007,so3).
%surgery_id(so100008,so2).
%surgery_id(so100009,so2).
%surgery_id(so100010,so2).
%surgery_id(so100011,so4).
%surgery_id(so100012,so2).
%surgery_id(so100013,so2).

assignment_surgery(so100001,d001).
assignment_surgery(so100002,d002).
assignment_surgery(so100003,d003).
%assignment_surgery(so100004,d001).
%assignment_surgery(so100004,d002).
%assignment_surgery(so100005,d002).
%assignment_surgery(so100005,d003).
%assignment_surgery(so100006,d001).
%assignment_surgery(so100007,d003).
%assignment_surgery(so100008,d004).
%assignment_surgery(so100008,d003).
%assignment_surgery(so100009,d002).
%assignment_surgery(so100009,d004).
%assignment_surgery(so100010,d003).
%assignment_surgery(so100011,d001).
%assignment_surgery(so100012,d001).
%assignment_surgery(so100013,d004).



agenda_operation_room(or1,20241028,[(520,579,so100000),(1000,1059,so099999)]).


free_agenda0([],[(0,1440)]).
free_agenda0([(0,Tfin,)|LT],LT1):-!,free_agenda1([(0,Tfin,)|LT],LT1).
free_agenda0([(Tin,Tfin,_)|LT],[(0,T1)|LT1]):- T1 is Tin-1,
    free_agenda1([(Tin,Tfin,_)|LT],LT1).

free_agenda1([(,Tfin,)],[(T1,1440)]):-Tfin\==1440,!,T1 is Tfin+1.
free_agenda1([(,,_)],[]).
free_agenda1([(,T,),(T1,Tfin2,_)|LT],LT1):-Tx is T+1,T1==Tx,!,
    free_agenda1([(T1,Tfin2,_)|LT],LT1).
free_agenda1([(,Tfin1,),(Tin2,Tfin2,_)|LT],[(T1,T2)|LT1]):-T1 is Tfin1+1,T2 is Tin2-1,
    free_agenda1([(Tin2,Tfin2,_)|LT],LT1).


adapt_timetable(D,Date,LFA,LFA2):-timetable(D,Date,(InTime,FinTime)),treatin(InTime,LFA,LFA1),treatfin(FinTime,LFA1,LFA2).

treatin(InTime,[(In,Fin)|LFA],[(In,Fin)|LFA]):-InTime=<In,!.
treatin(InTime,[(_,Fin)|LFA],LFA1):-InTime>Fin,!,treatin(InTime,LFA,LFA1).
treatin(InTime,[(_,Fin)|LFA],[(InTime,Fin)|LFA]).
treatin(_,[],[]).

treatfin(FinTime,[(In,Fin)|LFA],[(In,Fin)|LFA1]):-FinTime>=Fin,!,treatfin(FinTime,LFA,LFA1).
treatfin(FinTime,[(In,)|],[]):-FinTime=<In,!.
treatfin(FinTime,[(In,)|],[(In,FinTime)]).
treatfin(_,[],[]).


intersect_all_agendas([Name],Date,LA):-!,availability(Name,Date,LA).
intersect_all_agendas([Name|LNames],Date,LI):-
    availability(Name,Date,LA),
    intersect_all_agendas(LNames,Date,LI1),
    intersect_2_agendas(LA,LI1,LI).

intersect_2_agendas([],_,[]).
intersect_2_agendas([D|LD],LA,LIT):-    intersect_availability(D,LA,LI,LA1),
                    intersect_2_agendas(LD,LA1,LID),
                    append(LI,LID,LIT).

intersect_availability((,),[],[],[]).

intersect_availability((_,Fim),[(Ini1,Fim1)|LD],[],[(Ini1,Fim1)|LD]):-
        Fim<Ini1,!.

intersect_availability((Ini,Fim),[(_,Fim1)|LD],LI,LA):-
        Ini>Fim1,!,
        intersect_availability((Ini,Fim),LD,LI,LA).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)],[(Fim,Fim1)|LD]):-
        Fim1>Fim,!,
        min_max(Ini,Ini1,_,Imax),
        min_max(Fim,Fim1,Fmin,_).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)|LI],LA):-
        Fim>=Fim1,!,
        min_max(Ini,Ini1,_,Imax),
        min_max(Fim,Fim1,Fmin,_),
        intersect_availability((Fim1,Fim),LD,LI,LA).


min_max(I,I1,I,I1):- I<I1,!.
min_max(I,I1,I1,I).


remove_unf_intervals(_,[],[]).
remove_unf_intervals(TSurgery,[(Tin,Tfin)|LA],[(Tin,Tfin)|LA1]):-DT is Tfin-Tin+1,TSurgery=<DT,!,
    remove_unf_intervals(TSurgery,LA,LA1).
remove_unf_intervals(TSurgery,[_|LA],LA1):- remove_unf_intervals(TSurgery,LA,LA1).


schedule_first_interval(TSurgery,[(Tin,)|],(Tin,TfinS)):-
    TfinS is Tin + TSurgery - 1.

insert_agenda((TinS,TfinS,OpCode),[],[(TinS,TfinS,OpCode)]).
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(TinS,TfinS,OpCode),(Tin,Tfin,OpCode1)|LA]):-TfinS<Tin,!.
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(Tin,Tfin,OpCode1)|LA1]):-insert_agenda((TinS,TfinS,OpCode),LA,LA1).

insert_agenda_doctors(,,[]).
insert_agenda_doctors((TinS,TfinS,OpCode),Day,[Doctor|LDoctors]):-
    retract(agenda_staff1(Doctor,Day,Agenda)),
    insert_agenda((TinS,TfinS,OpCode),Agenda,Agenda1),
    assert(agenda_staff1(Doctor,Day,Agenda1)),
    insert_agenda_doctors((TinS,TfinS,OpCode),Day,LDoctors).

% Predicate for operation availability
availability_operation(OpCode,Room,Day,LPossibilities,LDoctors):-
    surgery_id(OpCode,OpType),
    surgery(OpType,,TSurgery,),
    findall(Doctor,assignment_surgery(OpCode,Doctor),LDoctors),
    intersect_all_agendas(LDoctors,Day,LA),
    agenda_operation_room1(Room,Day,LAgenda),
    free_agenda0(LAgenda,LFAgRoom),
    intersect_2_agendas(LA,LFAgRoom,LIntAgDoctorsRoom),
    remove_unf_intervals(TSurgery,LIntAgDoctorsRoom,LPossibilities).



% Predicate to select the busiest doctor
select_busiest_doctor(Doctors, Date, SelectedDoctor) :-
    findall(Doctor-occupancy(Doctor, Occupancy),
            (member(Doctor, Doctors),
             calculate_occupancy(Doctor, Date, Occupancy)),
            Occupancies),
    sort(2, @>=, Occupancies, [SelectedDoctor-|]).

  % Predicate to calculate the occupied time of a doctor on a specific day
calculate_occupied_time(Doctor, Date, OccupiedTime) :-
    findall(Duration,
            (assignment_surgery(_, Doctor),
             surgery(OpType, _, Duration, _),
             intersect_all_agendas([Doctor], Date, Agenda),
             member((_, EndTime), Agenda),
             EndTime > Duration),
            Durations),
    sumlist(Durations, OccupiedTime).


% Predicate to calculate the total available time of a doctor on a specific day
calculate_available_time(Doctor, Date, AvailableTime) :-
    timetable(Doctor, Date, (Start, End)),
    AvailableTime is End - Start.


% Predicate to calculate the occupancy of a doctor on a specific day
calculate_occupancy(Doctor, Date, OccupancyPercentage) :-
    calculate_occupied_time(Doctor, Date, OccupiedTime),
    calculate_available_time(Doctor, Date, AvailableTime),
    OccupancyPercentage is (OccupiedTime / AvailableTime) * 100.



schedule_surgery_heuristic(OpCode, Room, Day) :-
    select_busiest_doctor(Doctors, Date, SelectedDoctor),
    surgery_id(OpCode, OpType),
    surgery(OpType, _, TSurgery, _),
    EndTime is StartTime + TSurgery - 1,
    retract(agenda_operation_room1(Room, Day, Agenda)),
    insert_agenda((StartTime, EndTime, OpCode), Agenda, NewAgenda),
    assertz(agenda_operation_room1(Room, Day, NewAgenda)),
    insert_agenda_doctors((StartTime, EndTime, OpCode), Day, [EarliestDoctor]),
    findall(Doc, (assignment_surgery(OpCode, Doc), Doc \= EarliestDoctor), OtherDocs),
    insert_agenda_doctors((StartTime, EndTime, OpCode), Day, OtherDocs).


obtain_heuristic_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, TFinOp) :-
    get_time(Ti),
    retractall(agenda_staff1(_, _, _)),
    retractall(agenda_operation_room1(_, _, _)),
    retractall(availability(_, _, _)),
    findall(_, (
        agenda_staff(D, Day, Agenda),
        assertz(agenda_staff1(D, Day, Agenda))
    ), _),
    agenda_operation_room(Room, Day, Agenda),
    assert(agenda_operation_room1(Room, Day, Agenda)),
    findall(_, (
        agenda_staff1(D, Day, L),
        free_agenda0(L, LFA),
        adapt_timetable(D, Day, LFA, LFA2),
        assertz(availability(D, Day, LFA2))
    ), _),
    findall(OpCode, surgery_id(OpCode, _), LOC),
    schedule_all_surgeries_heuristic(LOC, Room, Day),
    agenda_operation_room1(Room, Day, FinalAgenda),
    findall(Doctor, assignment_surgery(_, Doctor), LDoctors1),
    remove_equals(LDoctors1, LDoctors),
    list_doctors_agenda(Day, LDoctors, LAgendas),
    reverse(FinalAgenda, ReversedAgenda),
    evaluate_final_time(ReversedAgenda, LOC, FinalTime),
    AgOpRoomBetter = FinalAgenda,
    LAgDoctorsBetter = LAgendas,
    TFinOp = FinalTime,
    get_time(Tf),
    T is Tf-Ti,
    write('Solution based on Busiest Doctor Heuristic:'), nl,
    write('Operation Room Schedule = '), write(AgOpRoomBetter), nl,
    write('Doctors Schedules = '), write(LAgDoctorsBetter), nl,
    write('Final Time = '), write(TFinOp), nl,
    write('Time to generate solution: '), write(T), write(' seconds'), nl.


schedule_all_surgeries_heuristic([], _, _).
schedule_all_surgeries_heuristic([OpCode|Rest], Room, Day) :-
    surgery_id(OpCode, OpType),
    surgery(OpType, _, TSurgery, _),
    availability_operation(OpCode, Room, Day, LPossibilities, _),
    schedule_first_interval(TSurgery, LPossibilities, (TinS, TfinS)),
    retract(agenda_operation_room1(Room, Day, Agenda)),
    insert_agenda((TinS, TfinS, OpCode), Agenda, Agenda1),
    assertz(agenda_operation_room1(Room, Day, Agenda1)),
    findall(Doctor, assignment_surgery(OpCode, Doctor), LDoctors),
    insert_agenda_doctors((TinS, TfinS, OpCode), Day, LDoctors),
    retractall(availability(_, Day, _)),
    findall(_, (
        agenda_staff1(D, Day, L),
        free_agenda0(L, LFA),
        adapt_timetable(D, Day, LFA, LFA2),
        assertz(availability(D, Day, LFA2))
    ), _),
    schedule_all_surgeries_heuristic(Rest, Room, Day).


evaluate_final_time([],_,1441).
evaluate_final_time([(,Tfin,OpCode)|],LOpCode,Tfin):-member(OpCode,LOpCode),!.
evaluate_final_time([_|AgR],LOpCode,Tfin):-evaluate_final_time(AgR,LOpCode,Tfin).

list_doctors_agenda(_,[],[]).
list_doctors_agenda(Day,[D|LD],[(D,AgD)|LAgD]):-agenda_staff1(D,Day,AgD),list_doctors_agenda(Day,LD,LAgD).

remove_equals([],[]).
remove_equals([X|L],L1):-member(X,L),!,remove_equals(L,L1).
remove_equals([X|L],[X|L1]):-remove_equals(L,L1).